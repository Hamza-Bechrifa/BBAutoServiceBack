using System;
using System.Collections.Generic;

namespace BB_Auto_service.BBAutoServiceModels
{
    public partial class Voiture
    {
        public int Id { get; set; }
        public string Matricule { get; set; }
        public string Type { get; set; }
        public string Modele { get; set; }
        public string DateMiseEnCirculation { get; set; }
        public string Vin { get; set; }
        public int? Kilometrage { get; set; }
        public int? Client { get; set; }
        public string Contact { get; set; }
    }
}
