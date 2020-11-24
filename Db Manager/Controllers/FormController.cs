using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HouseholdUserApplication.Db_Manager;
using HouseholdUserApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HouseholdUserApplication.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class FormController : ControllerBase
    {
        [HttpPost("sendEmail")]
        public IActionResult SendRequest(Form form)
        {
            int id = Int32.Parse(User.Claims.First(c => c.Type == "UserId").Value);
            try
            {
                User user = UserManager.GetUserById(id);
                EmailManager.SendEmail(user, form);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}