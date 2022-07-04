using System;
using System.Collections.Generic;

namespace main.DataLoad
{
    public partial class NewTable
    {
        public int Id { get; set; }
        public decimal MediaSalarial { get; set; }
        public string Doenca { get; set; } = null!;
        public int MediaIdade { get; set; }


        public static void InsertData(decimal MediaSalarial, int MediaIdade, string Doenca){
            var novo = new NewTable();
            novo.Doenca = Doenca;
            novo.MediaIdade = MediaIdade;
            novo.MediaSalarial = MediaSalarial;

            using(var context = new ets_dadosContext()){
                context.NewTables.Add(novo);
                context.SaveChanges();
            }
        }
    }
}
