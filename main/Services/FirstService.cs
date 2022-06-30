using System;
using main.DataSource;
using main.DataLoad;


namespace main.Services;

class FirstService{
        public static void SeparaDados()
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
                
                PacientesClasseEstado.SaveData(item.Pacientes,item.ClasseSocial,item.Estado);
            }
        }

    }
}