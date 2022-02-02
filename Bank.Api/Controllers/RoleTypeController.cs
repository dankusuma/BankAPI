using Bank.Api.ActionFilter;
using Bank.Core.Entity;
using Bank.Core.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Bank.Api.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class RoleTypeController : ControllerBase
    {

        private readonly IRepository _repository;
        public RoleTypeController(IRepository repository)
        {
            _repository = repository;
        }

        [Authorize]
        [ServiceFilter(typeof(AdminAccessOnly))]
        [HttpPost]
        public IActionResult Add(RoleType roleType)
        {
            _repository.Add(roleType);
            return Ok();
        }


        [Authorize]
        [ServiceFilter(typeof(AdminAccessOnly))]
        [HttpGet]
        public IActionResult Get()
        {
            List<RoleType> roleTypes = new List<RoleType>();
            roleTypes = _repository.List<RoleType>(null);
            return Ok(roleTypes);
        }



    }
}
