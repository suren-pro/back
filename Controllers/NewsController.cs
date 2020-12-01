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
    public class NewsController : ControllerBase
    {
        [HttpPost("addNews")]
        public IActionResult Add(News news)
        {
            try
            {
                NewsManager.AddNews(news);
                return Ok(news);
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("getNews")]
        public IActionResult GetNews()
        {
            try
            {
                var news = NewsManager.GetNews();
                return Ok(news);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}