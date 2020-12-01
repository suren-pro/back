using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HouseholdUserApplication.Models
{
    public class PaymentOrder
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string MDOrder { get; set; }
        public int PAN { get; set; }
        public int CVC { get; set; }
        public int Year { get; set; }
        public string Text { get; set; }
        public string Language { get; set; }
    }
}
