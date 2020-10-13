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
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HouseholdUserApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProfileController : ControllerBase
    {
        [HttpGet("getProfile")]
        public async Task<IActionResult> Get()
        {
            int id = Int32.Parse(User.Claims.First(c => c.Type == "UserId").Value);
            User user = UserManager.GetUserById(id);
            if (user != null)
            {
                user.Cards = await PaymentManager.GetCards(id);
                user.Cards = UserManager.SetCardColors(user.Cards,id);
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
        public async Task<IActionResult> Pay()
        {
            int id = Int32.Parse(User.Claims.First(c => c.Type == "UserId").Value);

            try
            {
                RegisterOrder registerOrder = new RegisterOrder(500);
                registerOrder.OrderNumber = (UserManager.CheckLastOrder() + 1) + "hou1se";
                OrderModel orderModel = await PaymentManager.RegisterOrder(registerOrder);
                string url = orderModel.FormUrl.Replace("_binding", "").Replace("  ", " ");
                return Ok(url);

            }
            catch
            {
                return BadRequest();

            }
        }
        [HttpPost("getOrderStatus")]
        public async Task<IActionResult> GetOrderStatus(string orderNumber)
        {
            OrderStatusModel orderStatus = await PaymentManager.GetOrderStatus(orderNumber);

            //ActivityManager.AddActivity(orderStatus);
            return Ok(orderStatus);

        }

        [HttpPost("addCard")]
        public async Task<IActionResult> AddCard()
        {
            int id = Int32.Parse(User.Claims.First(c => c.Type == "UserId").Value);
            try
            {
                RegisterOrder registerOrder = new RegisterOrder(id, 1*100);
                registerOrder.OrderNumber = (UserManager.CheckLastOrder() + 1) + "hou1asdsse";
                UserManager.AddOrder("Adding Card");
                OrderModel orderModel = await PaymentManager.RegisterOrder(registerOrder);
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
                RegisterOrder registerOrder = new RegisterOrder(id,payment.Amount*100);
                OrderModel orderModel = await PaymentManager.RegisterOrder(registerOrder);
                UserManager.AddOrder("Adding Card");
                string result = await PaymentManager.Pay(payment,orderModel.OrderId);
                OrderStatusModel orderStatus = await PaymentManager.GetOrderStatus(orderModel.OrderId);
                orderStatus.Date = DateTime.Now.ToString();
                ActivityManager.AddActivity(orderStatus);
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
        [HttpDelete("deleteCard")]
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