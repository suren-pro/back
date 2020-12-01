using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HouseholdUserApplication.Models.PayX
{
    public class Content
    {
        public int Merchant_id { get; set; }
        public string Token { get; set; }
        public string Px_transfer_id { get; set; }
        public string Qr_text { get; set; }
    }
}
