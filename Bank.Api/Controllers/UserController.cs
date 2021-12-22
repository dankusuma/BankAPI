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
                    return BadRequest();
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

        //[Authorize]
        //[ServiceFilter(typeof(AdminAccessOnly))]
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
            var user = _repository.List<User>().Find(x => x.USERNAME == login.USERNAME);

            var claims = new List<Claim>();

            if (user == null || !user.VerifyPassword(login.PASSWORD))
            {
                return Unauthorized();
            }

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


            return Ok(new JwtSecurityTokenHandler().WriteToken(token));
        }


    }
}
