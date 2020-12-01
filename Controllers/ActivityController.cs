using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Google.Protobuf;
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
            string base64 = Convert.ToBase64String(bytes);
            Response.ContentType = "application/pdf";
            Response.Body.WriteAsync(bytes);

        }
        [HttpPost("getReport")]
        public IActionResult GetReport(Dates dates)
        {
            try
            {
                int id = Int32.Parse(User.Claims.First(c => c.Type == "UserId").Value);
                Report report = ReportManager.GetReport(dates,id);
                return Ok(report);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("getBillingMonths")]
        public IActionResult GetBillingMonths()
        {
            //try
            //{
                int id = Int32.Parse(User.Claims.First(c => c.Type == "UserId").Value);
                List<Dates> dic = ActivityManager.GetActivityDates(id);
                return Ok(dic);
            //}
            //catch
            //{
            //    return BadRequest();
            //}
        }
        [HttpPost("GetActivitiesByDate")]
        public IActionResult GetActivitiesByDate([FromBody] Dates dates)
        {
            //try
            //{
                int id = Int32.Parse(User.Claims.First(c => c.Type == "UserId").Value);
            //List<Activity> activities = ActivityManager.GetActivitiesByDate(dates, id);
            //return Ok(activities);
            try
            {
                Report report = ReportManager.GetReport(dates, id);
                byte [] bytes = PdfCreator.createPDF(report);
                string base64 = Convert.ToBase64String(bytes);
                return Ok(base64);

            }
            catch
            {
                return BadRequest();
            }
            //}
           /* catch
            {
                return BadRequest();
            }*/
        }
    }
}