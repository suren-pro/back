using HouseholdUserApplication.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace HouseholdUserApplication.CardUtils
{
    public class PaymentManager
    {
        public async static Task<OrderModel> RegisterOrder(RegisterOrder registerOrder)
        {
            using (HttpClient http = new HttpClient())
            {
                string uri = $"https://ipay.arca.am/payment/rest/register.do?userName={registerOrder.Username}&password={registerOrder.Password}&amount={registerOrder.Amount}&language=ru&orderNumber={registerOrder.OrderNumber}&language=en&clientId={registerOrder.ClientId}&returnUrl={registerOrder.ReturnUrl}";


                HttpResponseMessage response = await http.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string s = await response.Content.ReadAsStringAsync();
                    OrderModel orderModel = JsonConvert.DeserializeObject<OrderModel>(s);
                    return (orderModel);
                }
                else
                {
                    return null;
                }
            }
        }
        public static async Task<List<Card>> GetCards(int id)
        {

            using (HttpClient http = new HttpClient())
            {
                Uri uri = new Uri($"https://ipay.arca.am/payment/rest/getBindings.do?password=18537506&userName=18537506_binding&clientId={id}");

                http.DefaultRequestHeaders.Add("user", "44");
                HttpResponseMessage response = await http.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string s = await response.Content.ReadAsStringAsync();
                    GetBindingsModel bindingsModel = JsonConvert.DeserializeObject<GetBindingsModel>(s);
                    return bindingsModel.Bindings;
                }
                else
                {
                    return null;
                }
            }
        }
        public static async Task<string> GetBindingId(string orderId)
        {

            using (HttpClient http = new HttpClient())
            {
                string uri = $"https://ipay.arca.am/payment/rest/getBindings.do?password=18537506&userName=18537506_binding&orderId={orderId}";


                HttpResponseMessage response = await http.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string s = await response.Content.ReadAsStringAsync();
                    OrderStatus orderStatus = JsonConvert.DeserializeObject<OrderStatus>(s);
                    return orderStatus.BindingId;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
