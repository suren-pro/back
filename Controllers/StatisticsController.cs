using System;
using System.Collections.Generic;
using System.Data;
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
    public class StatisticsController : ControllerBase
    {
     
        [HttpPost("GetUtilityPricesByDate")]
        public IActionResult GetUtilityPricesByDate([FromBody] Dates dates)
        {
            try
            {
                int id = Int32.Parse(User.Claims.First(c => c.Type == "UserId").Value);
                List<Point> points = StatisticsManager.GetPriceHistory(dates, id);
                return Ok(points);

            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPost("GetUtilityRecordsByDate")]
        public IActionResult GetUtilityRecordsByDate([FromBody] Dates dates)
        {
            try
            {
                DateTime date1 = new DateTime(dates.Years[0], dates.Months[0], 1);
                DateTime date2 = new DateTime(dates.Years[1], dates.Months[1], 1);
                int id = Int32.Parse(User.Claims.First(c => c.Type == "UserId").Value);
                List<Point> points = StatisticsManager.GetUtilityRecordsByDate(dates, id);
                return Ok(points);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPost("GetTotalCurrent")]
        public IActionResult GetTotalCurrent()
        {
            try
            {
                Dates dates = new Dates();
                dates.Years = new int[] { DateTime.Now.Year };
                dates.Months = new int[] { DateTime.Now.Month-1};

                int id = Int32.Parse(User.Claims.First(c => c.Type == "UserId").Value);
                Total total = StatisticsManager.GetTotal(dates, id);
                return Ok(total);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPost("GetTotal")]
        public IActionResult GetTotal([FromBody] Dates dates)
        {
            try
            {
                int id = Int32.Parse(User.Claims.First(c => c.Type == "UserId").Value);
                Total total = StatisticsManager.GetTotal(dates, id);
                return Ok(total);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPost("GetTotalGraph")]
        public IActionResult GetTotalGraph()
        {
            try
            {
                int id = Int32.Parse(User.Claims.First(c => c.Type == "UserId").Value);
                Total total = StatisticsManager.GetTotalGraph(id);
                return Ok(total);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPost("GetPricesByDate")]
        public IActionResult GetPricesByDate(Dates dates)
        {
            try
            {
                int id = Int32.Parse(User.Claims.First(c => c.Type == "UserId").Value);
                List<UtilityDetail> utilityDetails = StatisticsManager.GetUtilityDetails(dates, id);
                return Ok(utilityDetails);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPost("getTotalGraphByYear")]
        public IActionResult GetTotalGraphByYear(Dates dates)
        {
            try
            {
                int id = Int32.Parse(User.Claims.First(c => c.Type == "UserId").Value);
                Total total = StatisticsManager.GetTotalGraphByYear(dates, id);
                return Ok(total);
            }
            catch
            {
                return BadRequest();
            }
        }

    }
}