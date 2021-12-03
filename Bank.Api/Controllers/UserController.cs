using Bank.Core;
using Bank.Core.Interface;
using Bank.Core.Model;
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
        public UserController(IRepository repository,IConfiguration configuration)
        {
            _repository = repository;
            _configuration = configuration;
        }


        [HttpPost]
        public IActionResult Add(User user)
        {
            if (_repository.List<User>().Exists(x => x.username == user.username))
            {
                return BadRequest();
            }

            user.HashPassword();
            _repository.Add(user);
            return Ok();
        }

        [Authorize]
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
            var user = _repository.List<User>().Find(x => x.username == login.UserName);

            var claims = new List<Claim>();

            if (user == null || !user.VerifyPassword(login.Password))
            {
                return Unauthorized();
            }

            claims.Add(new Claim(ClaimTypes.Role,user.role_id.ToString()));
            claims.Add(new Claim(ClaimTypes.Name, user.username));


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
