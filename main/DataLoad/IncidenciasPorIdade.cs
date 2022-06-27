using System;
using System.Collections.Generic;

namespace main.DataLoad
{
    public partial class IncidenciasPorIdade
    {
        public int Id { get; set; }
        public string Estados { get; set; } = null!;
        public int QuantidadeOcorrencias { get; set; }
        public string NomeDoenca { get; set; } = null!;
        public int Idade { get; set; }
    }
}
