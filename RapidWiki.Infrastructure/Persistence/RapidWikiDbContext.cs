using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RapidWiki.Domain.Entities;

namespace RapidWiki.Infrastructure.Persistence;

public class RapidWikiDbContext : IdentityDbContext<IdentityUser<Guid>, IdentityRole<Guid>, Guid>
{
    public RapidWikiDbContext(DbContextOptions<RapidWikiDbContext> options) : base(options)
    {
    }

    public DbSet<Departamento> Departamentos { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Procedimento> Procedimentos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Departamento>()
            .HasIndex(d => d.Nome)
            .IsUnique();

        // Usuario <-> Departamento
        modelBuilder.Entity<Usuario>()
            .HasMany(u => u.Departamentos)
            .WithMany(d => d.Usuarios)
            .UsingEntity(j => j.ToTable("UsuarioDepartamentos"));


        // Procedimento <-> Departamento
        modelBuilder.Entity<Procedimento>()
            .HasMany(p => p.Departamentos)
            .WithMany(d => d.Procedimentos)
            .UsingEntity(j => j.ToTable("ProcedimentoDepartamentos"));


        // Procedimento -> Autor (Usuario)
        modelBuilder.Entity<Procedimento>()
            .HasOne(p => p.Autor)
            .WithMany(u => u.Procedimentos)
            .HasForeignKey(p => p.AutorId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
