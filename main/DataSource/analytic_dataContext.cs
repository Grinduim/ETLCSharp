using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace main.DataSource
{
    public partial class analytic_dataContext : DbContext
    {
        public analytic_dataContext()
        {
        }

        public analytic_dataContext(DbContextOptions<analytic_dataContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ClasseSocial> ClasseSocials { get; set; } = null!;
        public virtual DbSet<Diagnostico> Diagnosticos { get; set; } = null!;
        public virtual DbSet<Doenca> Doencas { get; set; } = null!;
        public virtual DbSet<Estado> Estados { get; set; } = null!;
        public virtual DbSet<Paciente> Pacientes { get; set; } = null!;
        public virtual DbSet<Regio> Regioes { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            optionsBuilder.UseSqlServer("Server=JVLPC0565;Database=analytic_data;Integrated Security=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ClasseSocial>(entity =>
            {
                entity.ToTable("classe_social");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.SalarioPiso)
                    .HasColumnType("decimal(38, 0)")
                    .HasColumnName("salario_piso");

                entity.Property(e => e.SalarioTeto)
                    .HasColumnType("decimal(38, 0)")
                    .HasColumnName("salario_teto");
            });

            modelBuilder.Entity<Diagnostico>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("diagnosticos");

                entity.Property(e => e.DataDiagnostico)
                    .HasColumnType("datetime")
                    .HasColumnName("data_diagnostico");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id");

                entity.Property(e => e.IdDoenca).HasColumnName("id_doenca");

                entity.Property(e => e.IdPaciente).HasColumnName("id_paciente");

                entity.HasOne(d => d.IdDoencaNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.IdDoenca)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("diagnosticos_doencas_FK");

                entity.HasOne(d => d.IdPacienteNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.IdPaciente)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("diagnosticos_FK");
            });

            modelBuilder.Entity<Doenca>(entity =>
            {
                entity.ToTable("doencas");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Nome)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("nome");
            });

            modelBuilder.Entity<Estado>(entity =>
            {
                entity.ToTable("estados");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IdRegiao).HasColumnName("id_regiao");

                entity.Property(e => e.Nome)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("nome");

                entity.HasOne(d => d.IdRegiaoNavigation)
                    .WithMany(p => p.Estados)
                    .HasForeignKey(d => d.IdRegiao)
                    .HasConstraintName("estados_FK");
            });

            modelBuilder.Entity<Paciente>(entity =>
            {
                entity.ToTable("pacientes");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IdClasseSocial).HasColumnName("id_classe_social");

                entity.Property(e => e.IdEstado).HasColumnName("id_estado");

                entity.Property(e => e.Idade).HasColumnName("idade");

                entity.Property(e => e.Nome)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("nome");

                entity.HasOne(d => d.IdClasseSocialNavigation)
                    .WithMany(p => p.Pacientes)
                    .HasForeignKey(d => d.IdClasseSocial)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("pacientes_FK");

                entity.HasOne(d => d.IdEstadoNavigation)
                    .WithMany(p => p.Pacientes)
                    .HasForeignKey(d => d.IdEstado)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("pacientes_FK_1");
            });

            modelBuilder.Entity<Regio>(entity =>
            {
                entity.ToTable("regioes");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.NomeRegiao)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("nome_regiao");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
