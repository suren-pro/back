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
    public class VotesController : ControllerBase
    {
        [HttpPut("addVote")]
        public IActionResult Vote (Option option)
        {
            int id = Int32.Parse(User.Claims.FirstOrDefault(c=>c.Type =="UserId").Value);
            try
            {
                VoteManager.Vote(new Vote
                {
                    UserId = id,
                    Option = option
                });
                return Ok();
            }
            catch
            {
                return BadRequest();
            }    
        }
    }
}