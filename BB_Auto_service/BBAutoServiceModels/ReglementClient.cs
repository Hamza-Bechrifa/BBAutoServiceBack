using System;
using System.Collections.Generic;

namespace BB_Auto_service.BBAutoServiceModels
{
    public partial class ReglementClient
    {
        public int Id { get; set; }
        public int? Client { get; set; }
        public double Montant { get; set; }
        public string Mode { get; set; }
        public string DateOperation { get; set; }
        public string DateReglement { get; set; }
        public int? OrdreDeReparation { get; set; }
        public int? FactureClient { get; set; }
    }
}
