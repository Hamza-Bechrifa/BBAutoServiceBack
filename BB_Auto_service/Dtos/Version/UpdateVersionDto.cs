using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gateway.Dtos.Version
{
    public class UpdateVersionDto
    {
        public string type { get; set; }
        public string description { get; set; }

        public string startDate { get; set; }

        public string endDate { get; set; }
        public List<string> idList { get; set; }


    }
}
