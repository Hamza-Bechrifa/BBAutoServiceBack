using System;
using System.Collections.Generic;

namespace BB_Auto_service.BBAutoServiceModels
{
    public partial class Article
    {
        public int Id { get; set; }
        public string Reference { get; set; }
        public string Designation { get; set; }
        public double PrixHt { get; set; }
        public int Tva { get; set; }
        public double Fodec { get; set; }
        public double PrixTtc { get; set; }
        public double Marge { get; set; }
        public double PrixPublic { get; set; }
        public double StockInitial { get; set; }
        public double StockReel { get; set; }
        public double StockAlerte { get; set; }
        public string Type { get; set; }
        public string ReferenceOrigine { get; set; }
        public string Marque { get; set; }
        public string Emplacement { get; set; }
    }
}
