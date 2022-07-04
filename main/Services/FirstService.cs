using System.Reflection;
using System;
using main.DataSource;
using main.DataLoad;
namespace main.Services;

class FirstService
{
    public static void DiagnosticosPorClassEMes()
    {
        Console.WriteLine("teste");
        using (var DataSource = new analytic_dataContext())
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
                DiagnosticosClasseMe.SaveData(item.QtdDiagnosticos, item.ClasseSocial, item.Mes);
            }

        }
    }

    public static void SeparaDados() // paciente Classe SocialEstado
    {
        using (var context = new analytic_dataContext())
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

                PacientesClasseEstado.SaveData(item.Pacientes, item.ClasseSocial, item.Estado);
            }
        }

    }

    public static void MediaSalarialDoençaIdade() {

        using(var context =  new analytic_dataContext()){

            var result  = context.Pacientes.Join(context.Diagnosticos,
            p=> p.Id,
            dg => dg.IdPaciente,
            (p,dg) => new {
                ClasseSocial = p.IdClasseSocial,
                Idade = p.Idade,
                Doenca = dg.IdDoenca,
            }
            ).Join(context.Doencas,
            p=> p.Doenca,
            doe => doe.Id,
            (p,doe) => new {
                ClasseSocial = p.ClasseSocial,
                Idade = p.Idade,
                Doenca = doe.Nome
            }
            ).Join(context.ClasseSocials,
            p=> p.ClasseSocial,
            cs => cs.Id,
            (p,cs) => new {
                MediaSalarial = ((cs.SalarioPiso + cs.SalarioTeto) / 2),
                Idade = p.Idade,
                Doenca = p.Doenca
            }
            ).GroupBy(q => q.Doenca)
            .Select( p => new {
                MediaSalarial = Math.Round(p.Average(q=> q.MediaSalarial), 2),
                MediaIdade =  (int) Math.Round(p.Average(q=> q.Idade),0),
                Doenca = p.Key
            });

          

            foreach (var item in result)
            {
                NewTable.InsertData(item.MediaSalarial, item.MediaIdade, item.Doenca);

            }
        }
    }
}