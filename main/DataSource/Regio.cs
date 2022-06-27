using System;
using System.Collections.Generic;

namespace main.DataSource
{
    public partial class Regio
    {
        public Regio()
        {
            Estados = new HashSet<Estado>();
        }

        public int Id { get; set; }
        public string? NomeRegiao { get; set; }

        public virtual ICollection<Estado> Estados { get; set; }
    }
}
