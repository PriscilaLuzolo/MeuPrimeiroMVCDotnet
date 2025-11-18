using System;
using System.Collections.Generic;
using MeuPrimeiroMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace MeuPrimeiroMVC.Contexts;

public partial class ProjetoTesteContext : DbContext
{
    public ProjetoTesteContext()
    {
    }

    public ProjetoTesteContext(DbContextOptions<ProjetoTesteContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Equipe> Equipes { get; set; }

    public virtual DbSet<Jogador> Jogadors { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=projetoTeste;Integrated Security=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Equipe>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Equipe__3214EC075CD5FE7A");

            entity.ToTable("Equipe");

            entity.Property(e => e.Imagem)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Nome)
                .HasMaxLength(120)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Jogador>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Jogador__3214EC07245CA866");

            entity.ToTable("Jogador");

            entity.Property(e => e.Email)
                .HasMaxLength(120)
                .IsUnicode(false);
            entity.Property(e => e.Nome)
                .HasMaxLength(120)
                .IsUnicode(false);
            entity.Property(e => e.Senha)
                .HasMaxLength(120)
                .IsUnicode(false);

            entity.HasOne(d => d.IdEquipeNavigation).WithMany(p => p.Jogadors)
                .HasForeignKey(d => d.IdEquipe)
                .HasConstraintName("FK__Jogador__IdEquip__398D8EEE");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
