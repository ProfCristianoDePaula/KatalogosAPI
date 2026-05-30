using Katalogos.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace Katalogos.Infrastructure.Data;

// O IdentityDbContext já traz todas as tabelas de segurança prontas para rodar no nosso PostgreSQL depois
public class KatalogosDbContext : IdentityDbContext<KatalogosUser>
{
    public KatalogosDbContext(DbContextOptions<KatalogosDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Regras restritas para o banco de dados (Evitando lixo na base)
        builder.Entity<KatalogosUser>()
            .Property(u => u.NomeCompleto)
            .IsRequired()
            .HasMaxLength(150);

        builder.Entity<KatalogosUser>()
            .Property(u => u.DocumentoCpf)
            .IsRequired()
            .HasMaxLength(14);
    }
}