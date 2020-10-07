using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HouseholdUserApplication.Models
{
    public class Payment
    {
        public int ErrorCode { get; set; }
        public string AcsUrl { get; set; }
        public string RedirectUrl { get; set; }
    }
}
