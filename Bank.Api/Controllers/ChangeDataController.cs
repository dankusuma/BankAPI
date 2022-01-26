using Bank.Core.Entity;
using Bank.Core.Interface;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Bank.Api.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class ChangeDataController : ControllerBase
    {
        private readonly IRepository _repository;
        private readonly List<User> _users;

        public ChangeDataController(IRepository rep)
        {
            _repository = rep;
            _users = _repository.List<User>(null);
        }
    }
}
