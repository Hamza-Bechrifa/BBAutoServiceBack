﻿using System;
using System.Collections.Generic;

namespace BB_Auto_service.BBAutoServiceModels
{
    public partial class DevisClient
    {
        public int Id { get; set; }
        public int? Client { get; set; }
        public int? Voiture { get; set; }
        public string DateCreation { get; set; }
        public string DateDocument { get; set; }
        public int? Kilometrage { get; set; }
        public string CommentaireInterne { get; set; }
        public string CommentaireExterne { get; set; }
        public double TotalHt { get; set; }
        public double TotalTtc { get; set; }
    }
}
