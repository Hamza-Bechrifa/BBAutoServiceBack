using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Gateway.Dtos.Account
{
    public class RegisterDto
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        [MinLength(8)]
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        [Required]
        public string Role { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public List<string> OrganizationsId { get; set; }
        public int? Type { get; set; }



        // public DateTime CreatedDate { get; set; }
        // public DateTime UpdatedDate { get; set; }

    }
}
