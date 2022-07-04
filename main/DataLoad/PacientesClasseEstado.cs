using System;
using System.Collections.Generic;
using main.Services;
using Microsoft.EntityFrameworkCore; 
namespace main.DataLoad
{
    public partial class PacientesClasseEstado
    {
        public int Id { get; set; }
        public int QuantidadePacientes { get; set; }
        public string ClasseSocial { get; set; } = null!;
        public string Estado { get; set; } = null!;



        public static void SaveData(int qtdPaciente,int IdClasseSocial, string nomeEstado){
             using (var context = new ets_dadosContext()){
                 var dado = new PacientesClasseEstado(){
                    QuantidadePacientes = qtdPaciente,
                    ClasseSocial = IdClasseSocial.ToString(),
                    Estado = nomeEstado
                 };
                 context.Add(dado);
                 context.SaveChanges();
             }

        }
    }
}
