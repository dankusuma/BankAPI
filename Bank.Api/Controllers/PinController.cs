using Bank.Core.Entity;
using Bank.Core.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bank.Api.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class PinController : ControllerBase
    {
        private readonly IRepository _repo;
        private readonly IConfiguration _config;

        private List<User> _users;
        private List<RefMaster> _refMasters, _validationList, _formatList;

        public PinController(IRepository repo, IConfiguration config)
        {
            _repo = repo;
            _config = config;

            _users = _repo.List<User>(null);
            _refMasters = _repo.List<RefMaster>(null).Where(x => x.MASTER_GROUP == "PIN").ToList();
            _validationList = _refMasters.Where(x => x.MASTER_CODE != "FORMAT").ToList();
            _formatList = _refMasters.Where(x => x.MASTER_CODE == "FORMAT").ToList();
        }

        private ObjectResult PinValidation(string mode, Pin pin, User user)
        {
            if (user == null) return Unauthorized("Username not registered.");

            if (mode == "status")
            {
                if (user.PIN != null)
                {
                    return null;
                }
                else
                {
                    return Unauthorized(false);
                }
            }

            var valMessage = _refMasters.Where(x => x.VALUE == pin.PIN).SingleOrDefault();

            if (valMessage != null)
            {
                return BadRequest(valMessage.MASTER_CODE_DESCRIPTION);
            }

            if (!pin.PIN.All(char.IsNumber))
            {
                return BadRequest("Only accept 6 digit numbers");
            }

            int length = int.Parse(_refMasters.Where(x => x.MASTER_CODE == "LENGTH").SingleOrDefault().VALUE);

            if (pin.LENGTH < length)
            {
                return BadRequest("Pin too short. Only accept 6 digit numbers.");
            }

            if (pin.LENGTH > length)
            {
                return BadRequest("Pin too long. Only accept 6 digit numbers.");
            }

            /// Validate FORMAT
            foreach (var item in _formatList)
            {
                string dt = user.BIRTH_DATE.ToString(item.VALUE);
                if (pin.PIN == dt) return BadRequest(item.MASTER_CODE_DESCRIPTION);
            }

            /// If mode change compare OLD and NEW password
            if (mode == "change")
            {
                if (user.PIN == pin.PIN_HASH)
                {
                    return BadRequest("New pin must be different from the old one.");
                }
            }

            return null;
        }

        [HttpPost]
        public IActionResult CreatePIN(Pin pinModel)
        {
            string mode = "create";

            try
            {
                var user = _users.Where(x => x.USERNAME == pinModel.USERNAME).SingleOrDefault();

                var validateMessage = PinValidation(mode, pinModel, user);

                if (validateMessage == null)
                {
                    /// IF no error found CREATE pin hash
                    user.PIN = pinModel.PIN_HASH;

                    /// UPDATE data to user
                    _repo.Update(user);

                    return Ok("PIN created successfully. Please re-login");
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
        public IActionResult ChangePIN(Pin pinModel)
        {
            string mode = "change";

            try
            {
                var user = _users.Where(x => x.USERNAME == pinModel.USERNAME).SingleOrDefault();

                var validateMessage = PinValidation(mode, pinModel, user);

                if (validateMessage == null)
                {
                    /// IF no error found CREATE pin hash
                    user.PIN = pinModel.PIN_HASH;

                    /// UPDATE data to user
                    _repo.Update(user);

                    return Ok("PIN updated successfully");
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

        [HttpGet]
        public IActionResult PINStatus(string username)
        {
            string mode = "status";
            Pin pin = new();

            try
            {
                var user = _users.Where(x => x.USERNAME == username).SingleOrDefault();

                var validateMessage = PinValidation(mode, pin, user);

                if (validateMessage == null)
                {
                    pin.USERNAME = username;
                    pin.PIN = user.PIN;

                    return Ok(pin.IS_PIN_CREATED);
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
