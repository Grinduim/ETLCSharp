using System;

using Microsoft.EntityFrameworkCore;

namespace main.DataSource;

class SourceContext{

    public DbSet<Paciente> pacientes {get; set;}
    public DbSet<regioes> regioes {get; set;}
    public DbSet<Estados> estados {get;set;}
    public DbSet<doencas>doencas {get;set;}
    public DbSet<diagnosticos> diagnosticos {get;set;}
    public DbSet<Class_social> class_social {get;set;}
    public void conn(){
        Console.WriteLine("Implemente a Source context e seus metodos de busca");
    }
}