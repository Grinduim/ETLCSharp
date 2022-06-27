using System;
using System.Collections.Generic;

namespace main.DataLoad
{
    public partial class DiagnosticosClasseMe
    {
        public int Id { get; set; }
        public int QuantidadeDiagnosticos { get; set; }
        public string ClasseSocial { get; set; } = null!;
        public int Mes { get; set; }
    }
}
