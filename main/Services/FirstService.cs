using System;
using main.DataLoad;
using main.DataSource;
namespace main.Services;

class FirstService{
    public static void first(){
        IncidenciaPorIdade();
    }


    
    public static void IncidenciaPorIdade(){

        var query = new object();

        using(var context = new analytic_dataContext()){
            var interval = 20; //years

            var SelectFaixa = context.Pacientes.Join(context.Diagnosticos, a => a.Id, b => b.IdPaciente,(a,b) => new{
                Idade = a.Idade,
                IdDiagnostico = b.Id,
                IdDoenca = b.IdDoenca,
                IdEstado = a.IdEstado
            }).Join(context.Estados, a =>a.IdEstado, b => b.Id,(a,b) => new{
                Idade = a.Idade,
                IdDiagnostico = a.IdDiagnostico,
                Estado = b.Nome,
                IdDoenca = a.IdDoenca,
            }).Join(context.Doencas, b => b.IdDoenca, c => c.Id, (b,c) => new{
                Idade = b.Idade,
                IdDiagnostic = b.IdDiagnostico,
                Estado = b.Estado,
                Doenca = c.Nome,
            }).GroupBy(y => new{
                faixaEtaria = ((y.Idade + interval - 1) / interval),
                y.Estado,
                y.Doenca
            })
            .OrderBy(y => y.Key.Estado)
            .ThenBy(n => n.Key.faixaEtaria)
            .ThenBy(n => n.Key.Doenca)
            .Select(y => new{
                Estado = y.Key.Estado, 
                FaixaEtaria=y.Key.faixaEtaria,
                Doenca = y.Key.Doenca,
                Count = y.Count(ap => ap.IdDiagnostic > -1)
            });



            foreach(var linha in SelectFaixa){
                Console.WriteLine(linha);
            }

            // using(var context2 = new ets_dadosContext())
            //  {
            //     var table = context2.IncidenciasPorIdades;
            //     foreach(var line in SelectFaixa)
            //     {
            //         string Faixa = "";
            //         switch(line.GroupIndex){
            //             case 1: 
            //                 Faixa = "0-20";
            //                 break;
            //         }
            //         var insert = new
            //         {
            //             FaixaEtaria = Faixa,
            //             Qtd = 
            //         }           
            //         table.Add()
            //     }
            // }
        }


        
    }


}

