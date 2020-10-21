using HouseholdUserApplication.Db_Manager;
using HouseholdUserApplication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace HouseholdUserApplication.CardUtils
{
    public class PaymentManager
    {
        public async static Task<OrderModel> RegisterOrder(RegisterOrder registerOrder)
        {
            registerOrder.OrderNumber = (UserManager.CheckLastOrder() + 1) + "poasdrder11";

            using (HttpClient http = new HttpClient())
            {
                string uri = $"https://ipay.arca.am/payment/rest/register.do?" +
                    $"userName={PaymentSettings.UserName}&password={PaymentSettings.Password}&amount={registerOrder.Amount}&language=ru&orderNumber={registerOrder.OrderNumber}&language=en&clientId={registerOrder.ClientId}&returnUrl={registerOrder.ReturnUrl}";
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
                Uri uri = new Uri($"https://ipay.arca.am/payment/rest/getBindings.do?" +
                    $"password={PaymentSettings.Password}&userName={PaymentSettings.UserName}&clientId={id}");
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
        public static async Task<string> Pay(Payment payment,string mdOrder)
        {
            string uri = $"https://ipay.arca.am/payment/rest/paymentOrderBinding.do?" +
                $"password={PaymentSettings.Password}&userName={PaymentSettings.UserName}&" +
                $"mdOrder={mdOrder}&bindingId={payment.BindingId}";

            using (HttpClient httpClient = new HttpClient())
            {
                HttpResponseMessage message = await httpClient.PostAsJsonAsync<Payment>(uri, payment);
                string status = await message.Content.ReadAsStringAsync();
                return status;
            }
        }
        public static async Task<OrderStatusModel> GetOrderStatus(string orderId)
        {
            
            string url = $"https://ipay.arca.am/payment/rest/getOrderStatusExtended.do?" +
                $"password={PaymentSettings.Password}" +
                $"&orderId={orderId}&userName={PaymentSettings.UserName}&language=ru";
            using (HttpClient httpClient = new HttpClient())
            {
                HttpResponseMessage message = await httpClient.GetAsync(url);
                if (message.IsSuccessStatusCode)
                {
                    string s = await message.Content.ReadAsStringAsync();
                    OrderStatusModel orderStatus = JsonConvert.DeserializeObject<OrderStatusModel>(s);
                    return orderStatus;
                }
                else
                    return null;
            }
        }
        public static async Task<string> UnbindCard(Card card)
        {
            string url = $"https://ipay.arca.am/payment/rest/unBindCard.do?" +
                $"password={PaymentSettings.Password}&userName={PaymentSettings.UserName}&" +
                $"bindingId={card.BindingId}";
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage message = await client.GetAsync(url);
                string status = await message.Content.ReadAsStringAsync();
                return status;
            }
        }
    }
}
