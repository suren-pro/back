using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HouseholdUserApplication.Models
{
    public class BillingDetails
    {
        public BillingDetail Services { get; set; }
        public BillingDetail Payment { get; set; }
        public Billing Billing { get; set; }
    }
}
