using System;
using System.Collections.Generic;

namespace main.DataLoad
{
    public partial class OcorrenciasClasseSocialRegiao
    {
        public int Id { get; set; }
        public int QuantidadeOcorrencias { get; set; }
        public string? NomeDoença { get; set; }
        public string ClasseSocial { get; set; } = null!;
        public string Regiao { get; set; } = null!;



         public static void SaveData(int QuantidadeOcorrencias,int IdClasseSocial, string Regiao, string NomeDoença){
             using (var context = new ets_dadosContext()){
                 var dado = new OcorrenciasClasseSocialRegiao(){
                    QuantidadeOcorrencias = QuantidadeOcorrencias,
                    ClasseSocial = IdClasseSocial.ToString(),
                    NomeDoença = NomeDoença,
                    Regiao = Regiao
                 };
                 context.Add(dado);
                 context.SaveChanges();
             }

        }
    }
}
