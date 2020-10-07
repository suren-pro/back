using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HouseholdUserApplication.Models
{
    public class Activity
    {
        public int Id { get; set; }
        public string ServiceName { get; set; }
        public double CardNumber { get; set; }
        public double Fee { get; set; }
        public double Total { get; set; }
        public string OrderId { get; set; }
        public string Rrn { get; set; }
        public string Customer { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }
        public Address Address { get; set; }
        public DateTime Date { get; set; }
    }
}
