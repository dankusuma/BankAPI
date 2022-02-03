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
        private List<User> _users;
        private List<RefMaster> _refMasters;

        public UserController(IRepository repository, IConfiguration configuration)
        {
            _repository = repository;
            _configuration = configuration;

            _users = _repository.List<User>(null);
            _refMasters = _repository.List<RefMaster>(null);
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
            if (_repository.List<User>(null).Exists(x => x.USERNAME == val.USERNAME))
            {
                return BadRequest();
            }
            else
            {
                return Ok();
            }
        }

        [HttpPost]
        public IActionResult isEmailDuplicate(Validate val)
        {
            if (_repository.List<User>(null).Exists(x => x.EMAIL == val.EMAIL))
            {
                return BadRequest();
            }
            else
            {
                return Ok();
            }
        }

        [HttpPost]
        public IActionResult isPhoneDuplicate(Validate val)
        {
            if (_repository.List<User>(null).Exists(x => x.PHONE == val.PHONE))
            {
                return BadRequest();
            }
            else
            {
                return Ok();
            }
        }

        [HttpPost]
        public IActionResult isNIKDuplicate(Validate val)
        {
            if (_repository.List<User>(null).Exists(x => x.NIK == val.NIK))
            {
                return BadRequest();
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

        [HttpGet]
        public IActionResult GetUserData()
        {
            List<User> users = _repository.List<User>(null);
            List<User> newUser = new();

            foreach (var item in users)
            {
                User user = new()
                {
                    NIK = item.NIK,
                    USERNAME = item.USERNAME,
                    EMAIL = item.EMAIL,
                    PHONE = item.PHONE
                };

                newUser.Add(user);
            }

            return Ok(newUser);
        }

        [HttpPost]
        public IActionResult Authenticate(LoginModel login)
        {
            var user = VerifyUser(login.USERNAME, login.PASSWORD);
            return Ok(GenerateJWTToken(user));
        }

        [HttpPost]
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

        [NonAction]
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

        [HttpGet]
        public UserViewModel GetUserStatus(string username)
        {
            User usr = _users.Where(x => x.USERNAME == username).SingleOrDefault();

            return new UserViewModel
            {
                Username = usr.USERNAME,
                PinStatus = string.IsNullOrEmpty(usr.PIN) ? false : true,
                IsValidate = usr.IS_VALIDATE,
                IsActive = usr.IS_ACTIVE,
            };
        }

        [HttpPatch]
        public IActionResult UpdateIsActive(string username)
        {
            try
            {
                var user = _users.Where(x => x.USERNAME == username).SingleOrDefault();

                ObjectResult validateMessage = null;

                if (user == null) validateMessage = Unauthorized("");

                if (validateMessage == null)
                {
                    /// UPDATE data to user
                    _repository.Update(user);

                    return Ok("");
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
    }
}
