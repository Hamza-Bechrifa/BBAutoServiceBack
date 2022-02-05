using BB_Auto_service.BBAutoServiceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BB_Auto_service.Dto
{
    public class DevisClientDTO
    {

        public int Id { get; set; }
        public int? Client { get; set; }
        public int? Voiture { get; set; }
        public string dateDocument { get; set; }
        public int? Kilometrage { get; set; }
        public string CommentaireInterne { get; set; }
        public string CommentaireExterne { get; set; }
        public List<DetailleDevisClient> Detaille { get; set; }
    }
}

