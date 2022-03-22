using System;
using System.Collections.Generic;

namespace BB_Auto_service.BBAutoServiceModels
{
    public partial class Client
    {
        public Client()
        {
            OrdreDeReparation = new HashSet<OrdreDeReparation>();
        }

        public int Id { get; set; }
        public string NomPrenom { get; set; }
        public string Cin { get; set; }
        public string Telephone { get; set; }
        public string Adresse { get; set; }
        public double? Solde { get; set; }
        public double? Plafond { get; set; }
        public string CodeExterne { get; set; }

        public virtual ICollection<OrdreDeReparation> OrdreDeReparation { get; set; }
    }
}
