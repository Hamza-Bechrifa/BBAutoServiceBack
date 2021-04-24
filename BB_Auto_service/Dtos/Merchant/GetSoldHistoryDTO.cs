using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gateway.Dtos.Merchant
{
    public class GetSoldHistoryDTO
    {
        public string datedebut { get; set; }
        public string dateFin { get; set; }
        public string etat { get; set; }
        public string idTerminal { get; set; }
    }
}
