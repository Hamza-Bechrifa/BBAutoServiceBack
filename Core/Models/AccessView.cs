using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Models
{
    public class AccessView
    {
        [Key]
        public string IdAccessView { get; set; }
        public string DescriptionAccessView { get; set; }

    }
}
