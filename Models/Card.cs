using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HouseholdUserApplication.Models
{
    public class Card
    {
        
        public string MaskedPan { get; set; }
        public int ExpiryDate { get; set; }
        public string CardHolderName { get; set; }
        public string BindingId { get; set; }
        public string Color { get; set; }
        public string Name { get; set; }
    }
}
