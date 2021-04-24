using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gateway.Dtos.Merchant
{
    public class AddSoldDto
    {
        public string idTerminal { get; set; }
        public string operation { get; set; }
        public string commentaire { get; set; }
        public Int64 montant { get; set; }
        

    }
    public class AddSoldConfirmationDto
    {
        public string idDemande { get; set; }
        public string operation { get; set; }    

    }
}
