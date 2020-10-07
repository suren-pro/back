using MySql.Data.MySqlClient.Memcached;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HouseholdUserApplication.Models
{
    public class RegisterOrder
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string OrderNumber { get; set; }
        public decimal Amount { get; set; }
        public string ReturnUrl { get; set; }
        public string Description { get; set; }
        public string Language { get; set; }
        public string PageView { get; set; }
        public int ClientId { get; set; }
        public RegisterOrder(int id,decimal ammount)
        {
            ClientId = id;
            Amount =ammount;
            Username = "18537506_binding";
            Password = "18537506";
            OrderNumber = "62";
            ReturnUrl = "https://www.youtube.com/";
        }
    }
}
