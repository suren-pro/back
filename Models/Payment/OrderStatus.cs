using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HouseholdUserApplication.Models
{
    public class OrderStatus
    {
       
        public string OrderId { get; set; }
        public string OrderNumber { get; set; }
        public double Amount { get; set; }
        public string Description { get; set; }
        public string Date { get; set; }
        public string  AuthRefNum { get; set; }
        public string BindingId { get; set; }
    }
}
