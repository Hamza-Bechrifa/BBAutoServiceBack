using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gateway.Dtos.Transaction
{
    public class AvisMerchantDto
    {
        public string date { get; set; }
        //public string merchant { get; set; }
        public List<string> magasins { get; set; }

    }
}
