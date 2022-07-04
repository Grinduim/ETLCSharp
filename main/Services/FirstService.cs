using System.Reflection;
using System;
using main.DataSource;
using main.DataLoad;
namespace main.Services;

class FirstService
{
    public static void Diagnosticos_Classe_Mes()
    {
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
                Console.WriteLine(item);
            }

        }
    }
    public static void Diagnosticos_Por_Classe()
    {
        using (var DataSource = new DataSource.analytic_dataContext())
        {
            var diagnosticos = DataSource.Diagnosticos.Join(DataSource.Pacientes,
            dg => dg.IdPaciente,
            p => p.Id,
            (dg, p) => new
            {
                IdClassSocial = p.IdClasseSocial,
                IdDoenca = dg.IdDoenca
            }
            ).Join(DataSource.Doencas,
            dg => dg.IdDoenca,
            de => de.Id,
            (dg, de) => new
            {
                Doenca = de.Nome,
                IdClassSocial = dg.IdClassSocial
            }).GroupBy(cs => new
            {
                cs.IdClassSocial,
            })
            .Select(q => new
            {
                QtdDiagnosticos = q.Count(dg => dg.Doenca != ""),
                ClasseSocial = q.Key.IdClassSocial
            }).OrderBy(x => x.ClasseSocial)
            .ToList();

            foreach (var item in diagnosticos)
            {
                DataLoad.DiagnosticosPorClasse.SaveData(item.QtdDiagnosticos, item.ClasseSocial);
                Console.WriteLine(item);
            }

        }
    }
    public static void Doenca_Idade_Regiao(){
        using(var context = new DataSource.analytic_dataContext())
        {
            var doenca = context.Doencas;
            var paciente = context.Pacientes;
            var estado = context.Estados;
            var regiao = context.Regioes;
            
            var dados = context.Diagnosticos.Join(doenca, di => di.IdDoenca, d => d.Id, (di,d) => new{
                diagnosticoId = di.Id,
                pacienteId = di.IdPaciente,
                nomeDoenca = d.Nome
            }).Join(paciente, di => di.pacienteId, p => p.Id, (di,p) => new{
                diagnosticoId = di.diagnosticoId,
                nomeDoenca = di.nomeDoenca,
                idadePaciente = p.Idade,
                estadoId = p.IdEstado
            }).Join(estado, di => di.estadoId, e => e.Id, (di,e) => new{
                diagnosticoId = di.diagnosticoId,
                nomeDoenca = di.nomeDoenca,
                idadePaciente = di.idadePaciente,
                idRegiao = e.IdRegiao
            }).Join(regiao, di => di.idRegiao, r => r.Id, (di,r) => new{
                diagnosticoId = di.diagnosticoId,
                nomeDoenca = di.nomeDoenca,
                idadePaciente = di.idadePaciente,
                nomeRegiao = r.NomeRegiao
            })
            .GroupBy(di => new
            {
                di.nomeDoenca,
                di.nomeRegiao
            })
            .Select(di => new{
                nomeDoenca = di.Key.nomeDoenca,
                mediaIdade = (int)di.Average(i => i.idadePaciente),
                nomeRegiao = di.Key.nomeRegiao
            });

            foreach(var item in dados){
                DataLoad.DoençaIdadeRegiao.SaveData(item.nomeDoenca, item.mediaIdade, item.nomeRegiao);
                Console.WriteLine(item);
            }
        }
    }
    public static void Incidencia_Por_Idade(){

        var query = new object();

        using(var context = new DataSource.analytic_dataContext()){
            var interval = 20; //years

            var Query = context.Pacientes.Join(context.Diagnosticos, a => a.Id, b => b.IdPaciente,(a,b) => new{
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
                NomeDoenca = y.Key.Doenca,
                QuantidadeOcorrencias = y.Count(ap => ap.IdDiagnostic > -1)
            });

            foreach(var item in Query){
                Console.WriteLine(item);
                DataLoad.IncidenciasPorIdade.SaveData(item.Estado, item.QuantidadeOcorrencias, item.NomeDoenca, item.FaixaEtaria);
                Console.WriteLine("Item Salvo");
            }
        }      
    }
    public static void MediaSalarial_Doença_Idade() { // vulgo NewTable

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
                Console.WriteLine(item);
            }
        }
    }
    public static void Ocorrencias_Classe_Social_Regiao() 
    {
        using (var context = new DataSource.analytic_dataContext())
        {
            var query = context.Diagnosticos
            .Join(context.Pacientes, pc => pc.IdPaciente, es => es.Id, (pc,es) => new{
                ClasseSocial = es.IdClasseSocial,
                IdEstado = es.IdEstado,
                IdDiagnostico = pc.Id,
                IdDoenca= pc.IdDoenca
            })
            .Join(context.Estados, pc => pc.IdEstado, es => es.Id, (pc, es) => new
            {
                IdRegiao = es.IdRegiao,
                ClasseSocial = pc.ClasseSocial,
                IdDiagnostico = pc.IdDiagnostico,
                IdDoenca= pc.IdDoenca
            }).Join(context.Regioes, pc => pc.IdRegiao, es => es.Id, (pc,es) => new{
                Regiao = es.NomeRegiao,
                ClasseSocial = pc.ClasseSocial,
                IdDiagnostico = pc.IdDiagnostico,
                IdDoenca= pc.IdDoenca
            }).Join(context.Doencas, pc => pc.IdDoenca, es => es.Id, (pc,es) => new{
                Regiao = pc.Regiao,
                ClasseSocial = pc.ClasseSocial,
                IdDiagnostico = pc.IdDiagnostico,
                NomeDoenca= es.Nome
            })
             .GroupBy(g => new
             {
                 g.ClasseSocial,
                 g.Regiao,
                 g.NomeDoenca
             })
             .Select(q => new
             {
                 Ocorrencias = q.Count(g => g.IdDiagnostico > -1),
                 ClasseSocial = q.Key.ClasseSocial,
                 Regiao = q.Key.Regiao,
                 NomeDoenca = q.Key.NomeDoenca      
             });
            foreach (var item in query)
            {
                DataLoad.OcorrenciasClasseSocialRegiao.SaveData(item.Ocorrencias, item.ClasseSocial, item.Regiao, item.NomeDoenca);
                Console.WriteLine(item);
            }
        }

    }
    public static void Pacientes_Classe_Estado() 
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
    
}

