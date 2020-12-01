using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HouseholdUserApplication.Models
{
    public class UtilityRecord
    {
        public int Id { get; set; }
        public int Value { get; set; }
        public DateTime Date { get; set; }
        public Utility Utility { get; set; }
        public Address Address { get; set; }
    }
}
