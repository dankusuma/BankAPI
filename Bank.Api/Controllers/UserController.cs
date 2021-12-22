using Bank.Api.ActionFilter;
using Bank.Core;
using Bank.Core.Entity;
using Bank.Core.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
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
            if (_repository.List<User>().Exists(x => x.USERNAME == user.USERNAME))
            {
                return BadRequest();
            }

            user.HashPassword();
            _repository.Add(user);
            return Ok();
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
            List<User> users = _repository.List<User>();
            return Ok(users);
        }

        private JwtSecurityToken JwtSecurityTokenGenerator(List<Claim> claims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            return new JwtSecurityToken(
                        issuer: _configuration["JWT:Issuer"],
                        audience: _configuration["JWT:Audience"],
                        expires: DateTime.Now.AddMinutes(15),
                        claims: claims,
                        signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                        );
        }

        [HttpPost]
        public IActionResult Authenticate(LoginModel login)
        {
            JwtSecurityToken token = null;
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
                int suspendedHours = int.Parse(setting.Find(x => x.MASTER_CODE == "MAX_HOLD").VALUE);

                #region Validation
                /// If login on suspend
                if (user.LOGIN_HOLD > DateTime.Now)
                    validationMessage = string.Format("Your account are suspend for {0} hour(s)", suspendedHours);

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
                        validationMessage = string.Format("Max login attempt exceeded! Account will suspended for {0} hour(s)", suspendedHours);
                    }
                    else
                    {
                        /// Update user
                        _repository.Update(user);
                        validationMessage = "Username / Password invalid!";
                    }
                }

                /// Once login success, reset Login Attempt to 0 
                user.LOGIN_FAILED = 0;
                /// Update user
                _repository.Update(user);
                #endregion

                if (validationMessage == "")
                {
                    claims.Add(new Claim(ClaimTypes.Role, user.USER_TYPE.ToString()));
                    claims.Add(new Claim(ClaimTypes.Name, user.USERNAME));

                    token = JwtSecurityTokenGenerator(claims);

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
    }
}
