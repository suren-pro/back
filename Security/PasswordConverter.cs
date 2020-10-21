using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HouseholdUserApplication.Security
{
    public class PasswordConverter
    {
        public static string GetHashedValue(string password)
        {
            if (password == null)
                return password;

            string result;
            using (SHA512 shaM = new SHA512Managed())
            {
                result = Convert.ToBase64String(shaM.ComputeHash(Encoding.UTF8.GetBytes(password)));
            }

            return result;
        }
    }
}
