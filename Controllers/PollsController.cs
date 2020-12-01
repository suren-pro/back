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
    [ApiController]
    [Authorize]
    public class PollsController : ControllerBase
    {
        [HttpPost("addPoll")]
        public IActionResult AddPoll(PollOption poll)
        {
            try
            {
                PollManager.AddPoll(poll);
                return Ok();
            }
            catch 
            {
                return BadRequest();
            }
        }
        [HttpGet("getPoll/{id?}")]
        public IActionResult GetPoll(int id)
        {
            try
            {
                PollOption pollOption = PollManager.GetPoll(id);
                return Ok(pollOption);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("getPolls")]
        public IActionResult GetPolls()
        {
          int userId = Int32.Parse(User.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
          try
          {
                List<PollOption> pollOptions = PollManager.GetPolls(userId);
                return Ok(pollOptions);
          }
          catch
          {
                return BadRequest();
          }
        }
    }
}