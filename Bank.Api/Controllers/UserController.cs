using Bank.Core.Interface;
using Bank.Core.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Bank.Api.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IRepository _repository;

        public UserController(IRepository repository)
        {
            _repository = repository;
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

        [HttpGet]
        public IActionResult Get()
        {
            List<User> users = new List<User>();
            users = _repository.List<User>();
            return Ok(users);
        }


    }
}
