using System;
using System.Collections.Generic;

namespace main.DataSource
{
    public partial class Estado
    {
        public Estado()
        {
            Pacientes = new HashSet<Paciente>();
        }

        public int Id { get; set; }
        public string? Nome { get; set; }
        public int? IdRegiao { get; set; }

        public virtual Regio? IdRegiaoNavigation { get; set; }
        public virtual ICollection<Paciente> Pacientes { get; set; }
    }
}
