using System;
using System.Collections.Generic;

namespace BB_Auto_service.BBAutoServiceModels
{
    public partial class BonDeReception
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string DateCreation { get; set; }
        public string DateDocument { get; set; }
        public int? Fournisseur { get; set; }
        public string CommentaireInterne { get; set; }
        public string CommentaireExterne { get; set; }
        public double TotalHt { get; set; }
        public double TotalTtc { get; set; }

        public virtual Fournisseur FournisseurNavigation { get; set; }
    }
}
