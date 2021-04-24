using System;
using System.Collections.Generic;

namespace Core.Models
{
    public partial class OrganizationInformationDefault
    {
        public string OrganizationId { get; set; }
        public int InformationId { get; set; }

        public virtual Information Information { get; set; }
        public virtual Organization Organization { get; set; }
    }
}
