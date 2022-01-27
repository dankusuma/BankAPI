using Bank.Core.Entity;
using Bank.Core.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Bank.Api.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class PasswordController : ControllerBase
    {
        private readonly IRepository _repo;
        private readonly IConfiguration _config;

        private List<User> _users;
        private List<RefMaster> _mailSetting;

        public PasswordController(IRepository repo, IConfiguration config)
        {
            _repo = repo;
            _config = config;

            _users = _repo.List<User>(null);
            _mailSetting = _repo.List<RefMaster>(null).FindAll(x => x.MASTER_GROUP == "EMAIL");
        }

        private ObjectResult ForgotPasswordValidation(ForgotPassword forgotPassword, User user)
        {
            if (user == null) return Unauthorized("Username not registered.");
            else forgotPassword.EMAIL = user.EMAIL;

            if (!forgotPassword.IS_EMAIL_VALID) return Unauthorized("Incorrect email format");

            return null;
        }

        private ObjectResult ChangePasswordValidation(string mode, ChangePassword changePassword, User user)
        {
            if (user == null) return Unauthorized("Username not registered");

            if (mode == "forget")
            {
                if (changePassword.IS_TOKEN_EXPIRED)
                {
                    return Unauthorized("Token expired");
                }
                if (changePassword.TOKEN != user.CHANGE_PASSWORD_TOKEN)
                {
                    return Unauthorized($"Invalid token.\nUserToken:{user.CHANGE_PASSWORD_TOKEN}, new token: {changePassword.TOKEN}");
                }
            }

            if (user.PASSWORD == changePassword.PASSWORD_HASH)
            {
                return Unauthorized("New password must be different from the old one.");
            }

            return null;
        }

        [HttpPost]
        public IActionResult ForgotPassword(ForgotPassword forgotPassword)
        {
            try
            {
                var user = _users.Where(x => x.EMAIL == forgotPassword.EMAIL).SingleOrDefault();

                var validateMessage = ForgotPasswordValidation(forgotPassword, user);

                if (validateMessage == null)
                {
                    return SendEmail(user, forgotPassword.EMAIL);
                }
                else
                {
                    return validateMessage;
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message.ToString());
            }
        }

        [HttpPatch]
        public IActionResult ChangePassword(ChangePassword changePassword)
        {
            try
            {
                /// Get OLD password using USERNAME
                User user = _users.Find(x => x.USERNAME == changePassword.USERNAME);

                var validationMessage = ChangePasswordValidation(changePassword.MODE, changePassword, user);

                if (validationMessage == null)
                {
                    user.PASSWORD = changePassword.PASSWORD_HASH;
                    user.CHANGE_PASSWORD_TOKEN = null;
                    user.CHANGE_PASSWORD_TOKEN_EXPIRATION = null;

                    /// Update user
                    _repo.Update(user);

                    return Ok("Password updated successfully");
                }
                else
                {
                    return validationMessage;
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message.ToString());
            }
        }

        private ObjectResult SendEmail(User user, string email)
        {
            string username = user.USERNAME.Trim();
            string userHash = user.HashValue(username);
            string userHashExpiration = DateTime.Now.AddMinutes(15).ToString("yyyyMMddHHmm");
            string mailTo = email;
            string mailFrom = _mailSetting.Find(x => x.MASTER_CODE == "MAIL_FROM").VALUE;
            string mailFromPassword = _mailSetting.Find(x => x.MASTER_CODE == "MAIL_FROM_PASSWORD").VALUE;
            string mailSubject = _mailSetting.Find(x => x.MASTER_CODE == "MAIL_SUBJECT_RESET").VALUE;
            string mailBodyTemplatePath = _mailSetting.Find(x => x.MASTER_CODE == "MAIL_BODY_TEMPLATE_PATH").VALUE;
            string mailLink = _mailSetting.Find(x => x.MASTER_CODE == "MAIL_LINK").VALUE;
            string mailSignature = _mailSetting.Find(x => x.MASTER_CODE == "MAIL_SIGNATURE").VALUE;

            /// Update CHANGE_PASSWORD_TOKEN field with new hash
            user.CHANGE_PASSWORD_TOKEN = userHash;
            user.CHANGE_PASSWORD_TOKEN_EXPIRATION = userHashExpiration;

            /// Update user
            _repo.Update(user);

            /// Path
            string templatePath;
            string MailText = "";

            templatePath = Directory.GetCurrentDirectory() + mailBodyTemplatePath;
            StreamReader str = new StreamReader(templatePath);
            MailText = str.ReadToEnd();
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
            string smtpServer = _mailSetting.Find(x => x.MASTER_CODE == "SMTP_SERVER").VALUE;
            int smtpPort = int.Parse(_mailSetting.Find(x => x.MASTER_CODE == "SMTP_PORT").VALUE);
            bool smtpSSL = _mailSetting.Find(x => x.MASTER_CODE == "SMTP_SSL").VALUE == "true" ? true : false;
            bool smtpDefaultCredentials = _mailSetting.Find(x => x.MASTER_CODE == "SMTP_DEFAULT_CREDENTIALS").VALUE == "true" ? true : false;

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

            return Ok(string.Format("Email sent to {0} successfully", email));
        }
    }
}
