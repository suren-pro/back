using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using HouseholdUserApplication.CardUtils;
using HouseholdUserApplication.Db_Manager;
using HouseholdUserApplication.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HouseholdUserApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProfileController : ControllerBase
    {
        string host = "";
        
        [HttpGet("getProfile")]
        public async Task<IActionResult> Get()
        {
            int id = Int32.Parse(User.Claims.First(c => c.Type == "UserId").Value);
            User user = UserManager.GetUserById(id);
            if (user != null)
            {
                user.Cards = await PaymentManager.GetCards(id);
                return Ok(user);
            }
            else
                return BadRequest();
        }
        [HttpPut("updateDetails")]
        public IActionResult Update(User user)
        {
           UserManager.Update(user);
           return Ok();
        }
        [HttpPost("pay")]
        public async Task<IActionResult> Pay(Payment payment)
        {
            int id = Int32.Parse(User.Claims.First(c => c.Type == "UserId").Value);
            string api = "api/profile/addActivity";
            try
            {
                string desc = "Payment with new card";
                Uri domain = new Uri(Request.GetDisplayUrl());
                Uri uri = new Uri(domain.Scheme + "://" + domain.Host + (domain.IsDefaultPort ? "" : ":" + domain.Port));
                host = uri.ToString();
                RegisterOrder registerOrder = new RegisterOrder(id, payment.Amount * 100, host, desc,api);
                OrderModel orderModel = await PaymentManager.RegisterOrder(registerOrder);
                UserManager.AddOrder(orderModel.OrderId);
                string url = orderModel.FormUrl.Replace("_binding", "").Replace("  ", " ");
               
                return Ok(url);

            }
            catch
            {
                return BadRequest();

            }
        }
        [HttpGet("addActivity")]
        public async Task<IActionResult> GetOrderStatus([FromQuery]string orderId)
        {

            int id = Int32.Parse(User.Claims.First(c => c.Type == "UserId").Value);

            OrderStatusModel orderStatus = await PaymentManager.GetOrderStatus(orderId);
            Uri domain = new Uri(Request.GetDisplayUrl());
            Uri uri = new Uri(domain.Scheme + "://" + domain.Host + (domain.IsDefaultPort ? "" : ":" + domain.Port));
            host = uri.ToString();
            orderStatus.Date = DateTime.Now.ToString();
            ActivityManager.AddActivity(orderStatus,id);
            return Redirect($"{host}profile/statment");

        }

        [HttpGet("addCard")]
        public async Task<IActionResult> AddCard()
        {
            Uri domain = new Uri(Request.GetDisplayUrl());
            Uri uri = new Uri(domain.Scheme + "://" + domain.Host + (domain.IsDefaultPort ? "" : ":" + domain.Port));
            host = uri.ToString();
            int id = Int32.Parse(User.Claims.First(c => c.Type == "UserId").Value);
            try
            {
                string desc = "Card registration";
                RegisterOrder registerOrder = new RegisterOrder(id, 10 * 100,host,desc);
                OrderModel orderModel = await PaymentManager.RegisterOrder(registerOrder);
                UserManager.AddOrder(orderModel.OrderId);
                string url = orderModel.FormUrl.Replace("_binding", "").Replace("  ", " ");
                return Ok(url);

            }
            catch
            {
                return BadRequest();
            }
        }
        

        [HttpPost("payBinding")]
        public async Task<IActionResult> PayWithBinging(Payment payment)
        {
            int id = Int32.Parse(User.Claims.First(c => c.Type == "UserId").Value);
            try
            {
                string desc = "Payment with binding";
                RegisterOrder registerOrder = new RegisterOrder(id,payment.Amount*100,host,desc);
                OrderModel orderModel = await PaymentManager.RegisterOrder(registerOrder);
                UserManager.AddOrder(orderModel.OrderId);
                string result = await PaymentManager.Pay(payment,orderModel.OrderId);
                OrderStatusModel orderStatus = await PaymentManager.GetOrderStatus(orderModel.OrderId);
                orderStatus.Date = DateTime.Now.ToString();
                ActivityManager.AddActivity(orderStatus,id);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        
        [HttpPost("checkPassword")]
        public IActionResult CheckPassword(Login login)
        {
            if (UserManager.UserExists(login.Username, login.Password) != -1)
                return Ok();
            else
            {
                return BadRequest();
            }
        }
        [HttpPost("updatePassword")]
        public IActionResult UpdatePassword(Login login)
        {
            int id = Int32.Parse(User.Claims.First(c => c.Type == "UserId").Value);
            try
            {
                UserManager.UpdatePassword(login.Password, id);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPost("deleteCard")]
        public async Task<IActionResult> DeleteCard(Card card)
        {

            int id = Int32.Parse(User.Claims.First(c => c.Type == "UserId").Value);
            try
            {
                await PaymentManager.UnbindCard(card);
                return Ok();
            }
            catch
            {
                return BadRequest();

            }
        }
    }
}