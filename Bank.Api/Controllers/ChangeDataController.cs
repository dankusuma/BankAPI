using Bank.Core.Entity;
using Bank.Core.Entity.ChangeData;
using Bank.Core.Interface;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text.Json;

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

        [HttpPatch]
        public IActionResult ChangeAddress(ChangeAddress add)
        {
            try
            {
                User user = _users.Find(x => x.USERNAME == add.USERNAME);

                if (user == null)
                {
                    return NotFound("User not found");
                }

                user.PROVINCE = add.PROVINCE;
                user.KABUPATEN_KOTA = add.KABUPATEN_KOTA;
                user.KECAMATAN = add.KECAMATAN;
                user.KELURAHAN = add.KELURAHAN;
                user.ADDRESS = add.ADDRESS;

                if (add.IsAddressValid())
                {
                    _repository.Update(user);
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok("Success");
        }

        [HttpPatch]
        public IActionResult ChangeJob(ChangeJob job)
        {
            string response = "";
            try
            {
                User user = _users.Find(x => x.USERNAME == job.USERNAME);

                if (user == null)
                {
                    return NotFound("User not found");
                }

                if (job.IsJobValid())
                {
                    _repository.Update(user);
                    response = JsonSerializer.Serialize<User>(user);
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok("Success");
        }
    }
}