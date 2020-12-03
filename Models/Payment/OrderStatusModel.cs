
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HouseholdUserApplication.Models
{
    public class OrderStatusModel
    {
       
        public string OrderId { get; set; }
        public string OrderNumber { get; set; }
        public string  OrderStatus { get; set; }
        public string Amount { get; set; }
        public string Description { get; set; }
        public string Date { get; set; }
        public string  AuthRefNum { get; set; }
        public CardAuthInfo CardAuthInfo { get; set; }
        public BindingInfo BindingInfo { get; set; }
    }
}
