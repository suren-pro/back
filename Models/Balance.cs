using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HouseholdUserApplication.Models
{
    public class Balance
    {
        public int Id { get; set; }
        public DateTime OpenDate { get; set; }
        public double Utilities { get; set; }
        public double Services { get; set; }
        private double totalBalance;
        public double TotalBalance
        {
            get
            {
                totalBalance = Utilities + Services;
                return totalBalance;
            }
            set
            {
                totalBalance = value;
            }
        }
        public DateTime CloseDate { get; set; }
    }

}
