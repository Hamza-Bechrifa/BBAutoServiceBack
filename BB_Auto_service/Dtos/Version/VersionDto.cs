using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Gateway.Dtos.Version
{
    public class VersionDto
    {
        public string IdVersion { get; set; }
        [Required]
        [MaxLength(50, ErrorMessage = "Longueur max doit être 50 caractères")]
        public string zipFile { get; set; }
        [Required]
        public int typeVersion { get; set; }
        public int statusVersion { get; set; }
        [Required]
        [MaxLength(50, ErrorMessage = "Longueur max doit être 50 caractères")]
        public string descriptionVersion { get; set; }
        public string dateCreationVersion { get; set; }
    }
}
