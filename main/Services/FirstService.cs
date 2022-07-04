using System;
using main.DataSource;
using main.DataLoad;

namespace main.Services;

class FirstService
{
    public static void DiagnosticosPorClassEMes()
    {
        Console.WriteLine("teste");
        using (var DataSource = new DataSource.analytic_dataContext())
        {
            var diagnosticos = DataSource.Diagnosticos.Join(DataSource.Pacientes,
            dg => dg.IdPaciente,
            p => p.Id,
            (dg, p) => new
            {
                IdClassSocial = p.IdClasseSocial,
                DataDiagnostico = dg.DataDiagnostico,
                IdDoenca = dg.IdDoenca
            }
            ).Join(DataSource.Doencas,
            dg => dg.IdDoenca,
            de => de.Id,
            (dg, de) => new
            {
                Doenca = de.Nome,
                IdClassSocial = dg.IdClassSocial,
                Mes = dg.DataDiagnostico.Month,
            }).GroupBy(cs => new
            {
                cs.IdClassSocial,
                cs.Mes
            })
            .Select(q => new
            {
                QtdDiagnosticos = q.Count(dg => dg.Doenca != ""),
                ClasseSocial = q.Key.IdClassSocial,
                Mes = q.Key.Mes

            }).OrderBy(x => x.Mes)
            .ToList();

            foreach (var item in diagnosticos)
            {
                DataLoad.DiagnosticosClasseMe.SaveData(item.QtdDiagnosticos, item.ClasseSocial, item.Mes);
            }

        }
    }

    public static void SeparaDados()
    {
        using (var context = new DataSource.analytic_dataContext())
        {
            var querry = context.Pacientes
            .Join(context.Estados, pc => pc.IdEstado, es => es.Id, (pc, es) => new
            {
                IdPaciente = pc.Id,
                NomeEstado = es.Nome,
                ClasseSocial = pc.IdClasseSocial
            })
             .GroupBy(g => new
             {
                 g.ClasseSocial,
                 g.NomeEstado
             })
             .Select(q => new
             {
                 Pacientes = q.Count(g => g.IdPaciente > -1),
                 ClasseSocial = q.Key.ClasseSocial,
                 Estado = q.Key.NomeEstado

             });
            foreach (var item in querry)
            {

                DataLoad.PacientesClasseEstado.SaveData(item.Pacientes, item.ClasseSocial, item.Estado);
            }
        }

    }


    
    public static void IncidenciaPorIdadePorEstado(){

        var query = new object();

        using(var context = new DataSource.analytic_dataContext()){
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
        }      
    }
}

