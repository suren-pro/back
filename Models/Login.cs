using HouseholdUserApplication.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HouseholdUserApplication.Models
{
    public class Login
    {
        public string Username { get; set; }
        private string password;
        public string Password
        {
            get 
            {
                return password;            
            }
            set 
            {
                // password = PasswordConverter.GetHashedValue(value);
                password = value;
            }
        }
    }
}
