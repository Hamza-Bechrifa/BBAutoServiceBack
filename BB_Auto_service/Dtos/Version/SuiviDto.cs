using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gateway.Dtos.Version
{
    public class SuiviDto
    {
        public string idMerchant { get; set; }
        public string idMagasin { get; set; }
        public string idTerminal { get; set; }
        public string Version { get; set; }
        
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }
}
