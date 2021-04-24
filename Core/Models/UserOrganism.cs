using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models
{
    public class UserOrganism
    {
        public string UserId { get; set; }
        public string OrganizationId { get; set; }
        public virtual Organization Organization { get; set; }
        public virtual User User { get; set; }



    }
}
