using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models
{
    public class UserAccess
    {
        //public string IdUserAccess { get; set; }
        public string IdUser { get; set; }
        public string IdAccessView { get; set; }
        public int ValueUserAccess { get; set; }
        public List<AccessView> Access { get; set; }
    }
}
