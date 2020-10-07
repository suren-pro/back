using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HouseholdUserApplication.Models
{
    public class Vote
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public Option Option { get; set; }
    }
}
