using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HouseholdUserApplication.Models
{
    public class Report
    {
        public Billing Billing { get; set; }
        public Balance Balance { get; set; }
        
        public User User { get; set; }
    }
}
