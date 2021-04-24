using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gateway.Dtos.Merchant
{
    public class MerchantListIndexDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string GwMerchantBankId { get; set; }
        public string GwMerchantStatus { get; set; }
        public string GwMerchantAbrev { get; set; }
        public string GwMerchantAdress { get; set; }
        public string GwMerchantMail { get; set; }
        public string GwMerchantPhone { get; set; }
        public int? GwMerchantCloseDayHour { get; set; }
        public int Mobilty { get; set; }

    }
}
