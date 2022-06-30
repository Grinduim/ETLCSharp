using System;
using main.DataSource;
using main.DataLoad;

namespace main.Services;

class DoencaIdadeRegiaoService{
    public static void doencaIdadeRegiaoService(){
        using(var context = new analytic_dataContext())
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
                var nomeDoenca = item.nomeDoenca;
                var mediaIdade = item.mediaIdade;
                var nomeRegiao = item.nomeRegiao;
                DoençaIdadeRegiao.SaveData(nomeDoenca, mediaIdade, nomeRegiao);
            }
        }
    }
}