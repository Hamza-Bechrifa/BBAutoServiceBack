using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
    public class Organization
    {
        public Organization()
        {
            //Users = new HashSet<User>();
            //OrganizationInformation = new HashSet<OrganizationInformation>();
            //OrganizationInformationDefault = new HashSet<OrganizationInformationDefault>();
            //OrganizationInformationDefaultDisplay = new HashSet<OrganizationInformationDefaultDisplay>();
            //OrganizationInformationDisplay = new HashSet<OrganizationInformationDisplay>();
        }
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Logo { get; set; }
        public int? OrganismTypeId { get; set; }
        //public int? Locked { get; set; }
        public string NumericCode { get; set; }
        public string AlphaCode { get; set; }
        public string OrganismRefId { get; set; }
        public string MerchantRefId { get; set; }

        public virtual OrganizationType OrganismType { get; set; }
        //public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<UserOrganism> UserOrganisations { get; set; }

        //public virtual ICollection<OrganizationInformation> OrganizationInformation { get; set; }
        //    public virtual ICollection<OrganizationInformationDefault> OrganizationInformationDefault { get; set; }
        //    public virtual ICollection<OrganizationInformationDefaultDisplay> OrganizationInformationDefaultDisplay { get; set; }
        //    public virtual ICollection<OrganizationInformationDisplay> OrganizationInformationDisplay { get; set; }
    }
}
