using System;
using System.Collections.Generic;

namespace BB_Auto_service.BBAutoServiceModels
{
    public partial class Fournisseur
    {
        public Fournisseur()
        {
            BonDeReception = new HashSet<BonDeReception>();
        }

        public int Id { get; set; }
        public string Nom { get; set; }
        public string MatriculeFiscale { get; set; }
        public string Telephone { get; set; }
        public string Adresse { get; set; }
        public double? Solde { get; set; }
        public string CodeWinsoft { get; set; }
        public double? Marge { get; set; }

        public virtual ICollection<BonDeReception> BonDeReception { get; set; }
    }
}
