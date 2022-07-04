using System;
using System.Collections.Generic;

namespace main.DataLoad
{
    public partial class ReiciendenciaMesesRegium
    {
        public int Id { get; set; }
        public int Reicidencia { get; set; }
        public string NomeDoenca { get; set; } = null!;
        public string Regiao { get; set; } = null!;
        public int Mes { get; set; }


        public static void InsertData(int reicidencia, string Nome, string regiao, int mes){
            using(var context = new ets_dadosContext()){
                var novo = new ReiciendenciaMesesRegium();
                novo.Reicidencia = reicidencia;
                novo.NomeDoenca = Nome;
                novo.Regiao = regiao;
                novo.Mes = mes;
                context.ReiciendenciaMesesRegia.Add(novo);
                context.SaveChanges();
            }
        }
    }
}
