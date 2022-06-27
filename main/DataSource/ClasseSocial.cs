using System;
using System.Collections.Generic;

namespace main.DataSource
{
    public partial class ClasseSocial
    {
        public ClasseSocial()
        {
            Pacientes = new HashSet<Paciente>();
        }

        public int Id { get; set; }
        public decimal SalarioPiso { get; set; }
        public decimal SalarioTeto { get; set; }

        public virtual ICollection<Paciente> Pacientes { get; set; }
    }
}
