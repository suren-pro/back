using HouseholdUserApplication.Models.PayX;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HouseholdUserApplication.PayXUtils
{
    public class PaymentManager
    {
        public static async Task<string> GenerateQr(string amount)
        {
            GenerateQrModel qrModel = new GenerateQrModel();
            qrModel.amount = amount;
            string jsonInString = JsonConvert.SerializeObject(qrModel);
            Uri uri = new Uri("https://online.payx.am/pos_api/get_link");
            using (HttpClient httpClient = new HttpClient())
            {
                HttpResponseMessage message = await httpClient.PostAsync(uri, new StringContent(jsonInString, Encoding.UTF8, "application/json"));
                string status = await message.Content.ReadAsStringAsync();
                return status;
            }
        }
    }
}
