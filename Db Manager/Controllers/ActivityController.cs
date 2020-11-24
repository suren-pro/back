using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HouseholdUserApplication.ActivityUtils;
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
    public class ActivityController : ControllerBase
    {
       
        [HttpGet("getActivity")]
        public IActionResult GetActivity()
        {
            try
            {
                List<Activity> activity = ActivityManager.GetActivity();
                return Ok(activity);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("getBilling")]
        public IActionResult GetBilling()
        {
            int id = Int32.Parse(User.Claims.First(c => c.Type == "UserId").Value);
            try
            {
                Billing billing = ActivityManager.GetBilling(id);
                return Ok(billing);
            }
            catch
            {
                return BadRequest();
            }
        }
       
        [HttpPost("downloadActivity")]
        public void DownloadPdf(Dates dates)
        {
            int id = Int32.Parse(User.Claims.First(c => c.Type == "UserId").Value);
            Report report = ReportManager.GetReport(dates, id);
            byte [] bytes=  PdfCreator.createPDF(report);
            Response.ContentType = "application/pdf";
            Response.Body.WriteAsync(bytes);

        }
        [HttpPost("GetActivitiesByDate")]
        public IActionResult GetActivitiesByDate([FromBody] Dates dates)
        {
            //try
            //{
                int id = Int32.Parse(User.Claims.First(c => c.Type == "UserId").Value);
                List<Activity> activities = ActivityManager.GetActivitiesByDate(dates, id);
                return Ok(activities);
            //}
           /* catch
            {
                return BadRequest();
            }*/
        }
    }
}