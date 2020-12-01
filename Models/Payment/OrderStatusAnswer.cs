using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HouseholdUserApplication.Models
{
    public class OrderStatusAnswer
    {
        public int OrderNumber { get; set; }
        public int ActionCode { get; set; }
        public string ActionCodeDescription { get; set; }
        public int BindingId { get; set; }
    }
}
