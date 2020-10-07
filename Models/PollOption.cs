using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HouseholdUserApplication.Models
{
    public class PollOption
    {
        public int Id { get; set; }
        public Poll Poll { get; set; }
        public List<Option> Options { get; set; }
        public VoteCheck Selected { get; set; }
    }
}
