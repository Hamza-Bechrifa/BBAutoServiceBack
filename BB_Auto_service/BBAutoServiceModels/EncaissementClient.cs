using System;
using System.Collections.Generic;

namespace BB_Auto_service.BBAutoServiceModels
{
    public partial class EncaissementClient
    {
        public int Id { get; set; }
        public int Client { get; set; }
        public double Montant { get; set; }
        public DateTime DateCreation { get; set; }
        public DateTime DateReglement { get; set; }
        public int? OrdreDeReparation { get; set; }
        public int? FactureClient { get; set; }
    }
}
