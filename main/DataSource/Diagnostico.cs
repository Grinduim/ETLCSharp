using System;
using System.Collections.Generic;

namespace main.DataSource
{
    public partial class Diagnostico
    {
        public int Id { get; set; }
        public int IdPaciente { get; set; }
        public int IdDoenca { get; set; }
        public DateTime DataDiagnostico { get; set; }

        public virtual Doenca IdDoencaNavigation { get; set; } = null!;
        public virtual Paciente IdPacienteNavigation { get; set; } = null!;
    }
}
