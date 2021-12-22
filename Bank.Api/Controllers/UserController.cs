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

        private string Validation(String mode, User user, LoginModel login)
        {
            /// Maximum failed login attempt
            int maxFailed = 3;
            int suspendedHours = 24;

            if (mode == "login")
            {
                /// If login on suspend
                if (user.LOGIN_HOLD > DateTime.Now)
                    return string.Format("Your account are suspend for {0} hour(s)", suspendedHours);

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
                        return string.Format("Max login attempt exceeded! Account will suspended for {0} hour(s)", suspendedHours);
                    }
                    else
                    {
                        /// Update user
                        _repository.Update(user);
                        return "Username / Password invalid!";
                    }
                }

                /// Once login success, reset Login Attempt to 0 
                user.LOGIN_FAILED = 0;
                /// Update user
                _repository.Update(user);
            }

            if (mode == "change_password")
            {
                if (user == null) return "Username not recognized";

                if (login.PASSWORD == login.NEWPASSWORD) return "Can't use the same password as the old password";
            }

            return "";
        }

        [HttpPost]
        public IActionResult Authenticate(LoginModel login)
        {
            JwtSecurityToken token = null;
            var claims = new List<Claim>();

            string validationMessage = "";

            try
            {
                var user = _repository.List<User>().Find(x => x.USERNAME == login.USERNAME);

                /// Validation function
                validationMessage = Validation("login", user, login);

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
