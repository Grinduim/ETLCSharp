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

        public static void SaveData(string nomeDoenca, int mediaIdade, string nomeRegiao){
            using(var context = new ets_dadosContext()){
                var dataDados = new DoençaIdadeRegiao(){
                    Doenca = nomeDoenca,
                    MediaIdade = mediaIdade,
                    Regiao = nomeRegiao
                };
                context.Add(dataDados);
                context.SaveChanges();
            }
        }
    }
}
