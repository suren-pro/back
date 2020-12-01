using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HouseholdUserApplication.Models
{
    public class Billing
    {
        public int Id { get; set; }
        public Address Address { get; set; }
        public Balance Balance { get; set; }
        public double Remain { get; set; }
        public double Payed { get; set; }
        public double TotalBilling { get; set; }
        public bool IsApproved { get; set; }
    }
}
