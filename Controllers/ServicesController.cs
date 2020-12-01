using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HouseholdUserApplication.Db_Manager;
using HouseholdUserApplication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HouseholdUserApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        [HttpGet("getAvailibleServices")]
        public IActionResult GetAvailibleServices()
        { 
            int id = Int32.Parse(User.Claims.First(c => c.Type == "UserId").Value);

            try
            {
                List<Service> services = ServiceManager.GetAvalibileServices(id);
                return Ok(services);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}