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
        public int amount { get; set; }
        public string comment { get; set; }
        public ProtocolData Protocol_data { get; set; }
        public string multi { get; set; }
        public GenerateQrModel(int userId)
        {
            merchant_id = Convert.ToInt32(PayXSettings.MerchantId);
            token = PayXSettings.Token;
            Protocol_data = new ProtocolData();
            Protocol_data.Input1 =userId.ToString();
            multi = "1";
        }

    }
}
