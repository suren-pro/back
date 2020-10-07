using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DinkToPdf;
using DinkToPdf.Contracts;
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
        private IConverter _converter;

        public ActivityController(IConverter converter)
        {
            _converter = converter;
        }
        [HttpGet("getActivity")]
        public IActionResult GetActivity()
        {
            try
            {
                var activity = ActivityManager.GetActivity();
                return Ok(activity);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("downloadActivity/{id}")]
        public void DownloadPdf(int id)
        {
            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 10 },
                DocumentTitle = "PDF Report",
                //Out = @"D:\PDFCreator\Employee_Report.pdf"  USE THIS PROPERTY TO SAVE PDF TO A PROVIDED LOCATION
            };
            Activity activity = ActivityManager.GetActivity(id);
            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = TemplateGenerator.HtmlString(activity),
                WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/css", "styles.css") },
            };

            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };

            byte[] file = _converter.Convert(pdf);
            // return File(file, "application/pdf", $"Activity-{activity.OrderId}.pdf");
            Response.ContentType = "application/pdf";
            Response.Body.WriteAsync(file);

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