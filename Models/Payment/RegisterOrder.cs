using Microsoft.AspNetCore.Http;
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
        public double Amount { get; set; }
        public string ReturnUrl { get; set; }
        public string Description { get; set; }
        public string Language { get; set; }
        public string PageView { get; set; }
        public int ClientId { get; set; }
        public RegisterOrder(double amount,string host)
        {
            Amount =amount;
            Username = "18537506_binding";
            Password = "18537506";
            ReturnUrl = $"{host}/acount/profile";
        }
        public  RegisterOrder(int clientId, double amount, string host,string description)
        {
            ClientId = clientId;
            Amount = amount;
            Username = "18537506_binding";
            Password = "18537506";
            Description = description;
            ReturnUrl = $"{host}profile/account";

        }
        public RegisterOrder(int clientId, double amount, string host, string description,string api)
        {
            ClientId = clientId;
            Amount = amount;
            Username = "18537506_binding";
            Password = "18537506";
            Description = description;
            ReturnUrl = $"{host}profile/statment";

        }

    }
}
