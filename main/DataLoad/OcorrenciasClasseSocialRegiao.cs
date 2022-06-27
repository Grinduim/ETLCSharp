using System;
using System.Collections.Generic;

namespace main.DataLoad
{
    public partial class OcorrenciasClasseSocialRegiao
    {
        public int Id { get; set; }
        public int QuantidadeOcorrencias { get; set; }
        public string? NomeDoença { get; set; }
        public string ClasseSocial { get; set; } = null!;
        public string Regiao { get; set; } = null!;
    }
}
