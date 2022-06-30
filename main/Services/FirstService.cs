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
                 DiagnosticosClasseMe.SaveData(item.QtdDiagnosticos,item.ClasseSocial,item.Mes);
            }

        }


    }
}