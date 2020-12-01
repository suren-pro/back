using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HouseholdUserApplication.Models
{
    public class Total
    {
        public List<List<object>> Utilities { get; set; }
        public Dictionary<string,int> Services { get; set; }
    }
}
