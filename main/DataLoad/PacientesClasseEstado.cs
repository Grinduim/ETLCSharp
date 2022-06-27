using System;
using System.Collections.Generic;

namespace main.DataLoad
{
    public partial class PacientesClasseEstado
    {
        public int Id { get; set; }
        public int QuantidadePacientes { get; set; }
        public string ClasseSocial { get; set; } = null!;
        public string Estado { get; set; } = null!;
    }
}
