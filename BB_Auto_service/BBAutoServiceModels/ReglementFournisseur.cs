using System;
using System.Collections.Generic;

namespace BB_Auto_service.BBAutoServiceModels
{
    public partial class ReglementFournisseur
    {
        public int Id { get; set; }
        public int? Fournisseur { get; set; }
        public double Montant { get; set; }
        public string Mode { get; set; }
        public string DateOperation { get; set; }
        public string DateReglement { get; set; }
        public int? BonDeReception { get; set; }
        public int? FactureFournisseur { get; set; }
    }
}
