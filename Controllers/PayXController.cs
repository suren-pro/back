using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HouseholdUserApplication.Models.PayX;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HouseholdUserApplication.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class PayXController : ControllerBase
    {
        [HttpGet("generateQR")]
        public async Task<IActionResult> GenerateQrCode([FromQuery] int amount)
        {
            int id = Int32.Parse(User.Claims.First(c => c.Type == "UserId").Value);
            string status = await PayXUtils.PaymentManager.GenerateQr(amount,id);
            QrResponse qrResponse = JsonConvert.DeserializeObject<QrResponse>(status);
            return Ok(qrResponse.Content.Qr_text);
        }
    }
}