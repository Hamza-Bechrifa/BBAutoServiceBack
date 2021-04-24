using System;
using System.Collections.Generic;

namespace BB_Auto_service.BBAutoServiceModels
{
    public partial class DetailleBr
    {
        public int Id { get; set; }
        public int? BonDeReception { get; set; }
        public int? Article { get; set; }
        public double PrixHt { get; set; }
        public double Quantite { get; set; }
        public double Remise { get; set; }
        public double Tva { get; set; }
        public double TotalTtc { get; set; }
    }
}
