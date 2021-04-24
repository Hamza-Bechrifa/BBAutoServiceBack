using System;
using System.Collections.Generic;

namespace Core.Models
{
    public partial class OrganizationType
    {
        public OrganizationType()
        {
           // AspNetRoles = new HashSet<User>();
            Organization = new HashSet<Organization>();
        }

        public int Id { get; set; }
        public string Type { get; set; }

        //public virtual ICollection<User> AspNetRoles { get; set; }
        public virtual ICollection<Organization> Organization { get; set; }
    }
}
