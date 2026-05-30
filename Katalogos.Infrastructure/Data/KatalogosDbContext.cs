using Katalogos.Domain.Entities; // Adicione este using
using Katalogos.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Katalogos.Infrastructure.Data;

public class KatalogosDbContext : IdentityDbContext<KatalogosUser>
{
    public KatalogosDbContext(DbContextOptions<KatalogosDbContext> options) : base(options) { }

    // NOSSA NOVA TABELA NO POSTGRESQL!
    public DbSet<Produto> Produtos { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Manda o EF ler aquele arquivo de configuração que acabamos de criar
        builder.ApplyConfigurationsFromAssembly(typeof(KatalogosDbContext).Assembly);
    }
}