using System;
using System.Collections.Generic;

namespace main.DataLoad
{
    public partial class ReiciendenciaMesesRegium
    {
        public int Id { get; set; }
        public int Reicidencia { get; set; }
        public string NomeDoenca { get; set; } = null!;
        public string Regiao { get; set; } = null!;
        public int Mes { get; set; }
    }
}
