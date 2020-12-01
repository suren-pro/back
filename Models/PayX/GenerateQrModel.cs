using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HouseholdUserApplication.Models.PayX
{
    public class GenerateQrModel
    {
        public int merchant_id { get; set; }
        public string token { get; set; }
        public string amount { get; set; }
        public string comment { get; set; }
        public GenerateQrModel()
        {
            merchant_id = Convert.ToInt32(PayXSettings.MerchantId);
            token = PayXSettings.Token;
        }

    }
}
