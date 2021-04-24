using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gateway.Dtos.UserAccess
{
    public class AccessItemDto
    {
        public int Bank { get; set; }
        public int Host { get; set; }

        public int Magasin { get; set; }

        public int Merchant { get; set; }

        public int Terminal { get; set; }
        public int Tpe { get; set; }

    }
}
