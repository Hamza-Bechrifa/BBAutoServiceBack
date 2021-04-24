using BB_Auto_service.BBAutoServiceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BB_Auto_service.Dto
{
    public class BonDeReceptionDTO
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Date { get; set; }
        public int? Fournisseur { get; set; }
        public string CommentaireInterne { get; set; }
        public string CommentaireExterne { get; set; }
        public double TotalHt { get; set; }
        public double TotalTtc { get; set; }
        public List<DetailleBr> DetailleBR { get; set; }
    }
}
