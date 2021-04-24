using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gateway.Dtos.Account
{
    public class UpdateRolesDto
    {
        public string Id { get; set; }
        public List<string> roles { get; set; }

    }
}
