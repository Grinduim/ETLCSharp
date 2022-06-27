using System;
using System.Collections.Generic;

namespace main.DataLoad
{
    public partial class DoençaIdadeRegiao
    {
        public int Id { get; set; }
        public int MediaIdade { get; set; }
        public string Doenca { get; set; } = null!;
        public string Regiao { get; set; } = null!;
    }
}
