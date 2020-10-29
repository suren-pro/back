using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HouseholdUserApplication.Db_Manager;
using HouseholdUserApplication.Models;
using HouseholdUserApplication.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace HouseholdUserApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        IConfiguration configuration;
        public LoginController(IConfiguration configuration,IHttpContextAccessor httpContextAccessor)
        {
            this.configuration = configuration;
            HttpContextAccessor = httpContextAccessor;
        }

        public IHttpContextAccessor HttpContextAccessor { get; }

        [HttpPost("login")]
        public IActionResult Login([FromBody] Login user)
        {
            var dbUser = Authentification.Login(user);
            if (dbUser != null)
            {
                string token = JwtGenerator.GenerateJSONWebToken(dbUser.Id);
                Response.Headers.Add("Token", token);
                return Ok();
            }
            else
            {
                return BadRequest("Your password or username is incorrect");
            }

        }
        [HttpPost("resetPassword")]
        public IActionResult ResetPassword(Login login)
        {
            try
            {
                User user = Authentification.UserLink(login.Username);
                string token = JwtGenerator.GenerateJSONWebToken(user.Id);
                Response.Headers.Add("Token", token);
                Uri domain = new Uri(Request.GetDisplayUrl());
                Uri uri =  new Uri(domain.Scheme + "://" + domain.Host + (domain.IsDefaultPort ? "" : ":" + domain.Port));
                string url = uri.ToString();
                 EmailManager.SendEmail(user,token,url);
                return Ok() ;
               
            }
            catch
            {
                return BadRequest();
            }
        }
        //[HttpPut("updatePassword")]
        //public IActionResult UpdatePassword(Login login)
        //{
        //    int id = Int32.Parse(User.Claims.First(c => c.Type == "UserId").Value);
        //    try
        //    {
        //        UserManager.UpdatePassword(login.Password,id);
        //        return Ok();
        //    }
        //    catch
        //    {
        //        return BadRequest();
        //    }
        //}
    }
}