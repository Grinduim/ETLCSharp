using System;
using System.Collections.Generic;

namespace main.DataLoad
{
    public partial class DiagnosticosPorClasse
    {
        public int Id { get; set; }
        public int QuantidadeDiagnosticos { get; set; }
        public int ClasseSocial { get; set; }



        public static void SaveData(int qtd, int ClasseSocial)
        {

            using (var context = new ets_dadosContext())
            {
                var obj = new DiagnosticosPorClasse();
                obj.QuantidadeDiagnosticos = qtd;
                obj.ClasseSocial = ClasseSocial;
                context.DiagnosticosPorClasses.Add(obj);

                context.SaveChanges();
            }

        }
    }
}
