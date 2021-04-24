using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gateway.Dtos.Organization
{
    public class OrganizationListDto
    {
        public string id { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string MerchantRefId { get; set; }
        public string OrganismRefId { get; set; }

    }
}
