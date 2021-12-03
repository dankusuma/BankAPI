using Bank.Core.Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Api.ActionFilter
{
    public class AdminAccessOnly : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {

            var header = context.HttpContext.Request.Headers["Authorization"].ToString();
            var jwttoken = header.Replace("Bearer ", "");
            var token = new JwtSecurityTokenHandler().ReadJwtToken(jwttoken);
            var accesslevel = token.Claims.First(c => c.Type == ClaimTypes.Role).Value;

            if (int.Parse(accesslevel) != ((int)AccessLevels.Admin))
            {
                context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
            }
        }
    }
}
