using System;

using Microsoft.EntityFrameworkCore;

namespace main.DataSource;

class SourceContext : DbContext
{

    public DbSet<Paciente> pacientes { get; set; }
    public DbSet<regioes> regioes { get; set; }
    public DbSet<Estados> estados { get; set; }
    public DbSet<doencas> doencas { get; set; }
    public DbSet<diagnosticos> diagnosticos { get; set; }
    public DbSet<Classe_social> class_social { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

        optionsBuilder.UseSqlServer("Data Source=" + Environment.MachineName + ";Initial Catalog=analytic_data; Integrated Security=True");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Classe_social>(u=> {
            u.HasKey(e=> e.id);
            u.Property(e=>e.salario_piso);
            u.Property(e=>e.salario_teto);
        });

        modelBuilder.Entity<regioes>(entity =>
        {
            entity.HasKey(e => e.id);
            entity.Property(e => e.nome_regiao);
        });

        modelBuilder.Entity<Estados>(entity =>
        {
            entity.HasKey(e => e.id);
            entity.Property(e => e.nome);
            entity.HasOne(e => e.regiao);
        }
        );

        modelBuilder.Entity<Paciente>(entity =>
        {
            entity.HasKey(e => e.id);
            entity.Property(e => e.nome);
            entity.Property(e => e.idade);
            entity.Property(e => e.id_estado);
            entity.Property(e => e.id_classe_Social);
        });

        modelBuilder.Entity<doencas>(u =>
        {
            u.HasKey(e=>e.id);
            u.Property(e => e.nome);
        });

        modelBuilder.Entity<diagnosticos>(u=> {
            u.HasKey(e=>e.id);
            u.HasOne(e=>e.paciente);
            u.HasOne(e=>e.doencas);
            u.Property(e=>e.data_diagnostico);
        });
    }
    public void conn()
    {
        Console.WriteLine("Implemente a Source context e seus metodos de busca");
    }
}