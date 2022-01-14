using Bank.Api.ActionFilter;
using Bank.Core;
using Bank.Core.Entity;
using Bank.Core.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
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
                if (_repository.List<User>(null).Exists(x => x.USERNAME == user.USERNAME || x.EMAIL == user.EMAIL))
                {
                    return BadRequest("Username or Email already exist");
                }

                user.HashPassword();
                //user.HashPin();
                _repository.Add(user);
                return Ok("Success");
            }
            else
            {
                return BadRequest(validation);
            }
        }

        [HttpPost]
        public IActionResult isUserDuplicate(Validate val)
        {
            if (_repository.List<User>(null).Exists(x => x.USERNAME != val.USERNAME))
            {
                return Unauthorized("Duplicate Username");
            }
            else
            {
                return Ok("Success");
            }
        }

        [HttpPost]
        public IActionResult isEmailDuplicate(Validate val)
        {
            if (_repository.List<User>(null).Exists(x => x.USERNAME == val.EMAIL))
            {
                return Unauthorized("Duplicate Email");
            }
            else
            {
                return Ok("Success");
            }
        }

        [HttpPost]
        public IActionResult isPhoneDuplicate(Validate val)
        {
            if (_repository.List<User>(null).Exists(x => x.PHONE == val.PHONE))
            {
                return Unauthorized("Duplicate Phone Number");
            }
            else
            {
                return Ok("Success");
            }
        }

        [HttpPost]
        public IActionResult isNIKDuplicate(Validate val)
        {
            if (_repository.List<User>(null).Exists(x => x.NIK == val.NIK))
            {
                return Unauthorized("Duplicate NIK");
            }
            else
            {
                return Ok();
            }
        }

        [HttpPost]
        public IActionResult Upload(Upload upload)
        {
            string maxPictureSize = _repository.List<RefMaster>(null).Find(x => x.MASTER_CODE == "MAX_PICTURE_UPLOAD_SIZE").VALUE;
            string maxVideoSize = _repository.List<RefMaster>(null).Find(x => x.MASTER_CODE == "MAX_VIDEO_UPLOAD_SIZE").VALUE;
            upload.convertToFile(); // Dapat dari FE bentuknya string base64, kita convert ke IFormFile
            upload.doUpload(maxPictureSize, maxVideoSize); // Upload ke folder, lokasi nya bisa lihat di model Upload.cs
            return Ok(upload.status);
        }

        [Authorize]
        [ServiceFilter(typeof(AdminAccessOnly))]
        [HttpGet]
        public IActionResult Get()
        {
            List<User> users = new List<User>(null);
            users = _repository.List<User>(null);

            string print = "";
            users.OrderByDescending(x => x.ID).ToList().ForEach(x => print += x.USERNAME + "-" + x.NAME);
            return Ok(print);
        }

        [HttpPost]
        public IActionResult Authenticate(LoginModel login)
        {
            var user = VerifyUser(login.USERNAME, login.PASSWORD);
            return Ok(GenerateJWTToken(user));
        }

        public User VerifyUser(string username, string password)
        {
            /// Get setting
            var setting = _repository.List<RefMaster>(null).FindAll(x => x.MASTER_GROUP == "SETTING");

            /// Get user list
            var user = _repository.List<User>(null).Find(x => x.USERNAME == username);

            /// Maximum failed login attempt
            int maxFailed = int.Parse(setting.Find(x => x.MASTER_CODE == "MAX_LOGIN").VALUE);

            /// Waiting time if failed login attempt exceeded
            double suspendedHours = int.Parse(setting.Find(x => x.MASTER_CODE == "MAX_HOLD").VALUE);

            string suspendedHoursText = suspendedHours >= 60 ? "hour(s)" : "minute(s)";

            double suspendedTime = suspendedHours >= 60 ? suspendedHours / 60 : suspendedHours;

            #region Validation
            if (user == null)
            {
                throw new UnauthorizedAccessException("Username / Password invalid!");
            }
            /// If login on suspend
            else if (user.LOGIN_HOLD > DateTime.Now)
            {
                throw new MethodAccessException(string.Format("Your account are suspend for {0} {1}", suspendedTime, suspendedHoursText));
            }
            /// If user is null OR wrong password
            else if (!user.VerifyPassword(password))
            {
                /// Set Increment by 1 for LOGIN_FAILED field
                user.LOGIN_FAILED = user.LOGIN_FAILED + 1;

                /// If failed attempt > max failed
                if (user.LOGIN_FAILED >= maxFailed)
                {
                    if (suspendedHours >= 60)
                    {
                        /// Set LOGIN_HOLD to DateTime.Now + 1 Hour
                        user.LOGIN_HOLD = DateTime.Now.AddHours(suspendedTime);
                    }
                    else
                    {
                        /// Set LOGIN_HOLD to DateTime.Now + 1 Hour
                        user.LOGIN_HOLD = DateTime.Now.AddMinutes(suspendedTime);
                    }

                    /// Update user
                    _repository.Update(user);
                    throw new UnauthorizedAccessException(string.Format("Max login attempt exceeded! Account will suspended for {0} {1}", suspendedTime, suspendedHoursText));
                }
                else
                {
                    /// Update user
                    _repository.Update(user);
                    throw new UnauthorizedAccessException("Username / Password invalid!");
                }
            }
            #endregion

            return user;
        }

        private string GenerateJWTToken(User user)
        {
            string pinStatus = "false";

            // VALIDATE pin if empty or null then return false other than that true
            if (user.PIN == "") pinStatus = "false";
            else if (user.PIN == null) pinStatus = "false";
            else pinStatus = "true";

            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Role, user.USER_TYPE.ToString()));
            claims.Add(new Claim(ClaimTypes.Name, user.USERNAME));
            claims.Add(new Claim("pin_status", pinStatus));

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

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpPost]
        public IActionResult ForgotPassword(ForgotPassword forgotPassword)
        {
            string validationMessage = "";
            try
            {
                #region Validation 1
                if (forgotPassword.EMAIL == "") validationMessage = "Can't fill in an empty email";
                else if (!forgotPassword.ValidateEmail()) validationMessage = "Incorrect email format";
                #endregion

                if (validationMessage == "")
                {
                    var user = _repository.List<User>(null).Find(x => x.EMAIL == forgotPassword.EMAIL);

                    #region Validation 2
                    if (user == null) validationMessage = "Unregistered email";
                    else if (user.EMAIL == "" || user.EMAIL == null) validationMessage = "Unregistered email";
                    #endregion

                    if (validationMessage == "")
                    {
                        #region Send Mail
                        /// Get setting
                        var mailSetting = _repository.List<RefMaster>(null).FindAll(x => x.MASTER_GROUP == "EMAIL");

                        /// Email
                        string username = user.USERNAME.Trim();
                        string userHash = user.HashValue(username);
                        string userHashExpiration = DateTime.Now.AddMinutes(15).ToString("yyyyMMddHHmm");
                        string mailTo = forgotPassword.EMAIL;
                        string mailFrom = mailSetting.Find(x => x.MASTER_CODE == "MAIL_FROM").VALUE;
                        string mailFromPassword = mailSetting.Find(x => x.MASTER_CODE == "MAIL_FROM_PASSWORD").VALUE;
                        string mailSubject = mailSetting.Find(x => x.MASTER_CODE == "MAIL_SUBJECT_RESET").VALUE;
                        string mailBodyTemplatePath = mailSetting.Find(x => x.MASTER_CODE == "MAIL_BODY_TEMPLATE_PATH").VALUE;
                        string mailLink = mailSetting.Find(x => x.MASTER_CODE == "MAIL_LINK").VALUE;
                        string mailSignature = mailSetting.Find(x => x.MASTER_CODE == "MAIL_SIGNATURE").VALUE;

                        /// Update CHANGE_PASSWORD_TOKEN field with new hash
                        user.CHANGE_PASSWORD_TOKEN = userHash;
                        user.CHANGE_PASSWORD_TOKEN_EXPIRATION = userHashExpiration;

                        /// Update user
                        _repository.Update(user);

                        /// Path
                        string templatePath = Directory.GetCurrentDirectory() + mailBodyTemplatePath;
                        StreamReader str = new StreamReader(templatePath);
                        string MailText = str.ReadToEnd();
                        str.Close();

                        /// Set Username
                        MailText = MailText.Replace("[username]", username);
                        /// Set Link
                        MailText = MailText.Replace("[link]", string.Format(mailLink, username, userHash, userHashExpiration));
                        /// Set Username
                        MailText = MailText.Replace("[teamname]", mailSignature);
                        /// Set Body Text
                        string mailBody = MailText;

                        /// SMTP
                        string smtpServer = mailSetting.Find(x => x.MASTER_CODE == "SMTP_SERVER").VALUE;
                        int smtpPort = int.Parse(mailSetting.Find(x => x.MASTER_CODE == "SMTP_PORT").VALUE);
                        bool smtpSSL = mailSetting.Find(x => x.MASTER_CODE == "SMTP_SSL").VALUE == "true" ? true : false;
                        bool smtpDefaultCredentials = mailSetting.Find(x => x.MASTER_CODE == "SMTP_DEFAULT_CREDENTIALS").VALUE == "true" ? true : false;

                        #region New SMTP Email Setting
                        var client = new MailKit.Net.Smtp.SmtpClient();
                        client.Connect(smtpServer, smtpPort, true);

                        /// Note: since we don't have an OAuth2 token, disable the XOAUTH2 authentication mechanism.
                        client.AuthenticationMechanisms.Remove("XOAUTH2");

                        /// Note: only needed if the SMTP server requires authentication
                        client.Authenticate(mailFrom, mailFromPassword);

                        var msg = new MimeMessage();
                        msg.From.Add(new MailboxAddress("Mail system", mailFrom));
                        msg.To.Add(new MailboxAddress("Dear user", mailTo));
                        msg.Subject = mailSubject;

                        var bodyBuilder = new BodyBuilder
                        {
                            HtmlBody = mailBody,
                            TextBody = "Test"
                        };

                        msg.Body = bodyBuilder.ToMessageBody();

                        client.Send(msg);
                        client.Disconnect(true);
                        #endregion

                        return Ok(string.Format("Email sent to {0} successfully", forgotPassword.EMAIL));
                        #endregion
                    }
                    else
                    {
                        return Unauthorized(validationMessage);
                    }
                }
                else
                {
                    return BadRequest(validationMessage);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }
        }

        [HttpPost]
        public IActionResult ChangePassword(ChangePassword changePassword)
        {
            string validationMessage = "";

            try
            {
                #region Validation
                if (changePassword == null)
                {
                    validationMessage = "Please provide complete data";
                }
                else if (changePassword.USERNAME == "" || changePassword.USERNAME == null)
                {
                    validationMessage = "Please provide username";
                }
                else if (changePassword.NEW_PASSWORD == "" || changePassword.PASSWORD == null)
                {
                    validationMessage = "Please provide new password";
                }

                /// Get OLD password using USERNAME
                User user = _repository.List<User>(null).Find(x => x.USERNAME == changePassword.USERNAME);

                if (user == null)
                {
                    validationMessage = "Username not registered";
                }
                else if (user.CHANGE_PASSWORD_TOKEN != changePassword.TOKEN && changePassword.MODE == "create")
                {
                    validationMessage = "Invalid token";
                }
                else if (user.VerifyPassword(changePassword.NEW_PASSWORD) == true)
                {
                    validationMessage = "New password must be different from the old one.";
                }
                #endregion

                if (validationMessage == "")
                {
                    user.PASSWORD = changePassword.NEW_PASSWORD;
                    user.HashPassword();

                    /// Update user
                    _repository.Update(user);

                    return Ok("Password updated successfully");
                }
                else
                {
                    return BadRequest(validationMessage);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }
        }

        [HttpPost]
        public IActionResult CreatePIN(Pin pin)
        {
            string validationMessage = "";
            pin.mode = "create";

            try
            {
                /// GET user data
                User user = _repository.List<User>(null).Find(x => x.USERNAME == pin.USERNAME);

                /// RETURN Unauthorized when username NOT FOUND
                if (user == null) return Unauthorized("Username not registered.");

                /// SET user to pin model for validation
                pin.user = user;

                /// Validation for PIN
                validationMessage = pin.PinValidation();

                if (validationMessage == "")
                {
                    /// IF no error found CREATE pin hash
                    user.PIN = pin.HashPIN(pin.PIN);

                    /// UPDATE data to user
                    _repository.Update(user);

                    return Ok("PIN created successfully. Please re-login");
                }
                else
                {
                    return BadRequest(validationMessage);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }
        }

        [HttpPost]
        public IActionResult ChangePIN(Pin pin)
        {
            string validationMessage = "";
            pin.mode = "change";

            try
            {
                /// Get OLD password using USERNAME
                User user = _repository.List<User>(null).Find(x => x.USERNAME == pin.USERNAME);

                /// RETURN Unauthorized when username NOT FOUND
                if (user == null) return Unauthorized("Username not registered.");

                /// SET user to pin model for validation
                pin.user = user;

                /// Validation for PIN
                validationMessage = pin.PinValidation();

                if (validationMessage == "")
                {
                    user.PIN = pin.HashPIN(pin.NEW_PIN);

                    /// Update user
                    _repository.Update(user);

                    return Ok("Password updated successfully");
                }
                else
                {
                    return BadRequest(validationMessage);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }
        }

        [HttpPost]
        public IActionResult PINStatus(Pin pin)
        {
            string validationMessage = "";
            string pinStatus = "false";
            pin.mode = "status";

            try
            {
                /// Get OLD password using USERNAME
                User user = _repository.List<User>(null).Find(x => x.USERNAME == pin.USERNAME);

                /// RETURN Unauthorized when username NOT FOUND
                if (user == null) return Unauthorized("Username not registered.");

                /// SET user to pin model for validation
                pin.user = user;

                /// Validation for PIN
                validationMessage = pin.PinValidation();

                if (validationMessage == "")
                {
                    /// VALIDATE pin if empty or null then return false other than that true
                    if (user.PIN == "") pinStatus = "false";
                    else if (user.PIN == null) pinStatus = "false";
                    else pinStatus = "true";

                    return Ok(pinStatus);
                }
                else
                {
                    return BadRequest(validationMessage);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }
        }
    }
}
