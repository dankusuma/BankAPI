using Bank.Api.ActionFilter;
using Bank.Core;
using Bank.Core.Entity;
using Bank.Core.Interface;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;

namespace Bank.Api.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IRepository _repository;
        private readonly IConfiguration _configuration;
        public UserController(IRepository repository, IConfiguration configuration)
        {
            _repository = repository;
            _configuration = configuration;
        }


        [HttpPost]
        public IActionResult Add(User user)
        {
            string validation = user.dataValidation();
            if (validation.Equals("Success"))
            {
                if (_repository.List<User>().Exists(x => x.USERNAME == user.USERNAME))
                {
                    return BadRequest("Username already exist");
                }

                user.HashPassword();
                user.HashPin();
                _repository.Add(user);
                return Ok("Success");
            }
            else
            {
                return Ok("Error400");
            }

        }
        [HttpPost]
        public IActionResult isUserDuplicate(string username)
        {
            if (_repository.List<User>().Exists(x => x.USERNAME == username))
            {
                return Ok("Duplicate Username");
            }
            return Ok("Success");
        }

        [HttpPost]
        public IActionResult isEmailDuplicate(string email)
        {
            if (_repository.List<User>().Exists(x => x.EMAIL == email))
            {
                return Ok("Duplicate Email");
            }
            return Ok("Success");
        }

        [HttpPost]
        public IActionResult Upload(Upload upload)
        {
            upload.convertToFile(); // Dapat dari FE bentuknya string base64, kita convert ke IFormFile
            upload.doUpload(); // Upload ke folder, lokasi nya bisa lihat di model Upload.cs
            return Ok(upload.status);
        }

        [Authorize]
        [ServiceFilter(typeof(AdminAccessOnly))]
        [HttpGet]
        public IActionResult Get()
        {
            List<User> users = new List<User>();
            users = _repository.List<User>();
            return Ok(users);
        }

        [HttpPost]
        public IActionResult Authenticate(LoginModel login)
        {
            var claims = new List<Claim>();

            string validationMessage = "";

            try
            {
                /// Get setting
                var setting = _repository.List<RefMaster>().FindAll(x => x.MASTER_GROUP == "SETTING");

                /// Get user list
                var user = _repository.List<User>().Find(x => x.USERNAME == login.USERNAME);

                /// Maximum failed login attempt
                int maxFailed = int.Parse(setting.Find(x => x.MASTER_CODE == "MAX_LOGIN").VALUE);

                /// Waiting time if failed login attempt exceeded
                double suspendedHours = int.Parse(setting.Find(x => x.MASTER_CODE == "MAX_HOLD").VALUE);

                string suspendedHoursText = suspendedHours >= 60 ? "hour(s)" : "minute(s)";

                suspendedHours = suspendedHours >= 60 ? suspendedHours / 60 : suspendedHours;

                #region Validation
                /// If login on suspend
                if (user.LOGIN_HOLD > DateTime.Now)
                    validationMessage = string.Format("Your account are suspend for {0} {1}", suspendedHours, suspendedHoursText);

                /// If user is null OR wrong password
                if (user == null || !user.VerifyPassword(login.PASSWORD))
                {
                    /// Set Increment by 1 for LOGIN_FAILED field
                    user.LOGIN_FAILED = user.LOGIN_FAILED + 1;

                    /// If failed attempt > max failed
                    if (user.LOGIN_FAILED >= maxFailed)
                    {
                        /// Set LOGIN_HOLD to DateTime.Now + 1 Hour
                        user.LOGIN_HOLD = DateTime.Now.AddHours(1);

                        /// Update user
                        _repository.Update(user);
                        validationMessage = string.Format("Max login attempt exceeded! Account will suspended for {0} {1}", suspendedHours, suspendedHoursText);
                    }
                    else
                    {
                        /// Update user
                        _repository.Update(user);
                        validationMessage = "Username / Password invalid!";
                    }
                }
                #endregion

                if (validationMessage == "")
                {
                    claims.Add(new Claim(ClaimTypes.Role, user.USER_TYPE.ToString()));
                    claims.Add(new Claim(ClaimTypes.Name, user.USERNAME));

                    var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                    var token = new JwtSecurityToken(
                        issuer: _configuration["JWT:Issuer"],
                        audience: _configuration["JWT:Audience"],
                        expires: DateTime.Now.AddMinutes(15),
                        claims: claims,
                        signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                        );

                    /// Once login success, reset Login Attempt to 0 
                    user.LOGIN_FAILED = 0;

                    /// Update user
                    _repository.Update(user);

                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                }
                else
                {
                    return Unauthorized(validationMessage);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }
        }


        [HttpPost]
        public IActionResult ForgotPassword(ForgotPassword forgotPassword)
        {
            try
            {
                if (forgotPassword.EMAIL != "")
                {
                    OkObjectResult res = (OkObjectResult)isEmailDuplicate(forgotPassword.EMAIL);
                    var user = _repository.List<User>().Find(x => x.EMAIL == forgotPassword.EMAIL);

                    if (res.Value.ToString() != "Success")
                    {
                        #region Send Mail
                        try
                        {
                            /// Get setting
                            var mailSetting = _repository.List<RefMaster>().FindAll(x => x.MASTER_GROUP == "EMAIL");

                            /// Email
                            string userName = user.USERNAME.Trim();
                            string mailTo = forgotPassword.EMAIL;
                            string mailFrom = mailSetting.Find(x => x.MASTER_CODE == "MAIL_FROM").VALUE;
                            string mailFromPassword = mailSetting.Find(x => x.MASTER_CODE == "MAIL_FROM_PASSWORD").VALUE;
                            string mailSubject = mailSetting.Find(x => x.MASTER_CODE == "MAIL_SUBJECT_RESET").VALUE;
                            string mailBodyTemplatePath = mailSetting.Find(x => x.MASTER_CODE == "MAIL_BODY_TEMPLATE_PATH").VALUE;
                            string mailLink = mailSetting.Find(x => x.MASTER_CODE == "MAIL_LINK").VALUE;
                            string mailSignature = mailSetting.Find(x => x.MASTER_CODE == "MAIL_SIGNATURE").VALUE;

                            /// Path
                            string templatePath = Directory.GetCurrentDirectory() + mailBodyTemplatePath;
                            var x1 = Directory.GetDirectoryRoot(templatePath);
                            StreamReader str = new StreamReader(templatePath);
                            string MailText = str.ReadToEnd();
                            str.Close();

                            /// Set Username
                            MailText = MailText.Replace("[username]", userName);
                            /// Set Link
                            MailText = MailText.Replace("[link]", mailLink);
                            /// Set Username
                            MailText = MailText.Replace("[teamname]", mailSignature);
                            /// Set Body Text
                            string mailBody = MailText;

                            /// SMTP
                            string smtpServer = mailSetting.Find(x => x.MASTER_CODE == "SMTP_SERVER").VALUE;
                            int smtpPort = int.Parse(mailSetting.Find(x => x.MASTER_CODE == "SMTP_PORT").VALUE);
                            bool smtpSSL = mailSetting.Find(x => x.MASTER_CODE == "SMTP_SSL").VALUE == "true" ? true : false;

                            MailMessage mail = new();
                            mail.From = new MailAddress(mailFrom);
                            mail.To.Add(mailTo);
                            mail.Subject = mailSubject;
                            mail.Body = mailBody;
                            mail.IsBodyHtml = true;

                            SmtpClient SmtpServer = new SmtpClient(smtpServer);
                            SmtpServer.Port = smtpPort;
                            SmtpServer.Credentials = new System.Net.NetworkCredential(mailFrom, mailFromPassword);
                            SmtpServer.EnableSsl = smtpSSL;

                            SmtpServer.Send(mail);

                            return Ok(string.Format("Email sent to {0} successfully", forgotPassword.EMAIL));
                        }
                        catch (Exception ex)
                        {
                            //return BadRequest(string.Format("Failed to sent email to {0}", forgotPassword.EMAIL));
                            return BadRequest(ex.Message.ToString());
                        }
                        #endregion
                    }
                    else
                    {
                        return Unauthorized("Email not registered");
                    }
                }
                else
                {
                    return BadRequest("Must provide email");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }
        }
    }
}
