using System;
using System.Collections.Generic;

namespace main.DataLoad
{
    public partial class DiagnosticosClasseMe
    {
        public int Id { get; set; }
        public int QuantidadeDiagnosticos { get; set; }
        public string ClasseSocial { get; set; } = null!;
        public int Mes { get; set; }


        public static void SaveData(int qtd, int ClasseSocial, int Mes)
        {

            using (var context = new ets_dadosContext())
            {
                var obj = new DiagnosticosClasseMe();
                obj.QuantidadeDiagnosticos = qtd;
                obj.ClasseSocial = ClasseSocial.ToString();
                obj.Mes = Mes;
                context.DiagnosticosClasseMes.Add(obj);

                context.SaveChanges();
            }

        }
    }
}
