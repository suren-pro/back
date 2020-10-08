using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HouseholdUserApplication.Models
{
    public class OrderStatus
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public int OrderId { get; set; }
        public int OrderNumber { get; set; }
        public string BindingId { get; set; }
    }
}
