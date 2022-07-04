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

    public static void SeparaDados() // paciente Classe SocialEstado
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

    public static void MediaSalarialDoençaIdade()
    {

        using (var context = new analytic_dataContext())
        {

            var result = context.Pacientes.Join(context.Diagnosticos,
            p => p.Id,
            dg => dg.IdPaciente,
            (p, dg) => new
            {
                ClasseSocial = p.IdClasseSocial,
                Idade = p.Idade,
                Doenca = dg.IdDoenca,
            }
            ).Join(context.Doencas,
            p => p.Doenca,
            doe => doe.Id,
            (p, doe) => new
            {
                ClasseSocial = p.ClasseSocial,
                Idade = p.Idade,
                Doenca = doe.Nome
            }
            ).Join(context.ClasseSocials,
            p => p.ClasseSocial,
            cs => cs.Id,
            (p, cs) => new
            {
                MediaSalarial = ((cs.SalarioPiso + cs.SalarioTeto) / 2),
                Idade = p.Idade,
                Doenca = p.Doenca
            }
            ).GroupBy(q => q.Doenca)
            .Select(p => new
            {
                MediaSalarial = Math.Round(p.Average(q => q.MediaSalarial), 2),
                MediaIdade = (int)Math.Round(p.Average(q => q.Idade), 0),
                Doenca = p.Key
            });



            foreach (var item in result)
            {
                NewTable.InsertData(item.MediaSalarial, item.MediaIdade, item.Doenca);

            }
        }
    }

    public static void IncidenciaPorFaixaEtariaPorEstado()
    {

        var query = new object();

        using (var context = new DataSource.analytic_dataContext())
        {
            var interval = 20; //years

            var Query = context.Pacientes.Join(context.Diagnosticos, a => a.Id, b => b.IdPaciente, (a, b) => new
            {
                Idade = a.Idade,
                IdDiagnostico = b.Id,
                IdDoenca = b.IdDoenca,
                IdEstado = a.IdEstado
            }).Join(context.Estados, a => a.IdEstado, b => b.Id, (a, b) => new
            {
                Idade = a.Idade,
                IdDiagnostico = a.IdDiagnostico,
                Estado = b.Nome,
                IdDoenca = a.IdDoenca,
            }).Join(context.Doencas, b => b.IdDoenca, c => c.Id, (b, c) => new
            {
                Idade = b.Idade,
                IdDiagnostic = b.IdDiagnostico,
                Estado = b.Estado,
                Doenca = c.Nome,
            }).GroupBy(y => new
            {
                faixaEtaria = ((y.Idade + interval - 1) / interval),
                y.Estado,
                y.Doenca
            })
            .OrderBy(y => y.Key.Estado)
            .ThenBy(n => n.Key.faixaEtaria)
            .ThenBy(n => n.Key.Doenca)
            .Select(y => new
            {
                Estado = y.Key.Estado,
                FaixaEtaria = y.Key.faixaEtaria,
                NomeDoenca = y.Key.Doenca,
                QuantidadeOcorrencias = y.Count(ap => ap.IdDiagnostic > -1)
            });

            foreach (var item in Query)
            {
                Console.WriteLine(item);
                DataLoad.IncidenciasPorIdade.SaveData(item.Estado, item.QuantidadeOcorrencias, item.NomeDoenca, item.FaixaEtaria);
                Console.WriteLine("Item Salvo");
            }
        }
    }

    public static void Reincidencia_Classe_Mes()
    {

        using (var context = new analytic_dataContext())
        {
            var result = context.Pacientes.Join(context.Estados,
                p => p.IdEstado,
                etd => etd.Id,
                (p, etd) => new
                {
                    IdRegiao = etd.IdRegiao,
                    IdPaciente = p.Id,
                }).Join(context.Regioes,
                    p => p.IdRegiao,
                    reg => reg.Id,
                    (p, reg) => new
                    {
                        Regiao = reg.NomeRegiao,
                        Paciente = p.IdPaciente
                    }).Join(context.Diagnosticos,
                    p => p.Paciente,
                    dg => dg.IdPaciente,
                    (p, dg) => new
                    {
                        Mes = dg.DataDiagnostico.Month,
                        Regiao = p.Regiao,
                        Doenca = dg.IdDoenca
                    }
                ).Join(context.Doencas,
                 p => p.Doenca,
                doe => doe.Id,
                (p, doe) => new
                {
                    Mes = p.Mes,
                    Regiao = p.Regiao,
                    Doenca = doe.Nome
                }
                ).GroupBy(q => new
                {
                    q.Mes,
                    q.Regiao,
                    q.Doenca
                }).Select(q => new
                {
                    reincidencia = q.Select(y => y.Doenca).Count(),
                    Mes = q.Key.Mes,
                    regiao = q.Key.Regiao,
                    doenca = q.Key.Doenca

                }).Where(p => p.reincidencia > 1);

            foreach (var item in result)
            {
                ReiciendenciaMesesRegium.InsertData(item.reincidencia, item.doenca, item.regiao, item.Mes);
            }
        }
    }
}