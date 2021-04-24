using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models
{
    public class User : IdentityUser
    {
        public int IsFirstConnection { get; set; }
        public DateTime LastConnection { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        //public string Role { get; set; }
        //public string OrganizationId { get; set; }
       // public Organization Organization { get; set; }

        public virtual List<UserOrganism> UserOrganizations { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
        //public int OrganismIdentitiesId { get; set; }


    }
}
