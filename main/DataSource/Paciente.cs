using System;
using System.Collections.Generic;

namespace main.DataSource
{
    public partial class Paciente
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
        public int Idade { get; set; }
        public int IdEstado { get; set; }
        public int IdClasseSocial { get; set; }

        public virtual ClasseSocial IdClasseSocialNavigation { get; set; } = null!;
        public virtual Estado IdEstadoNavigation { get; set; } = null!;
    }
}
