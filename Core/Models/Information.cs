using System;
using System.Collections.Generic;

namespace Core.Models
{
    public partial class Information
    {
        public Information()
        {
            OrganizationInformation = new HashSet<OrganizationInformation>();
            OrganizationInformationDefault = new HashSet<OrganizationInformationDefault>();
            OrganizationInformationDefaultDisplay = new HashSet<OrganizationInformationDefaultDisplay>();
            OrganizationInformationDisplay = new HashSet<OrganizationInformationDisplay>();
        }

        public int InformationId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<OrganizationInformation> OrganizationInformation { get; set; }
        public virtual ICollection<OrganizationInformationDefault> OrganizationInformationDefault { get; set; }
        public virtual ICollection<OrganizationInformationDefaultDisplay> OrganizationInformationDefaultDisplay { get; set; }
        public virtual ICollection<OrganizationInformationDisplay> OrganizationInformationDisplay { get; set; }
    }
}
