using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HouseholdUserApplication.Models
{
    public class Balance
    {
        public int Id { get; set; }
        public int TotalBalance { get; set; }
        public int Gas { get; set; }
        public int Water { get; set; }
        public int Electricity { get; set; }
        public int UtilityBalance { get; set; }
        public int UtilityPenalty{ get; set; }
        public int UtilityBalanceDate { get; set; }
        public int CommunityBalance { get; set; }
        public int CommunityPenalty { get; set; }
        public int CommunityBalanceDate { get; set; }
    }
}
