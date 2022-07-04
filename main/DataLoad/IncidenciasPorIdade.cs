using System;
using System.Collections.Generic;

namespace main.DataLoad
{
    public partial class IncidenciasPorIdade
    {
        public int Id { get; set; }
        public string Estados { get; set; } = null!;
        public int QuantidadeOcorrencias { get; set; }
        public string NomeDoenca { get; set; } = null!;
        public int Idade { get; set; }


        public static void SaveData(string estados, int quantidadeOcorrencias, string nomeDoenca, int idade)
        {

            using (var context = new ets_dadosContext())
            {
                var obj = new IncidenciasPorIdade();
                obj.Estados = estados;
                obj.QuantidadeOcorrencias = quantidadeOcorrencias;
                obj.NomeDoenca = nomeDoenca;
                obj.Idade = idade;
                context.IncidenciasPorIdades.Add(obj);

                context.SaveChanges();
            }

        }
    }
}
