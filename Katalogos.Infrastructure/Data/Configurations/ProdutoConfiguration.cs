using Katalogos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Katalogos.Infrastructure.Data.Configurations;

public class ProdutoConfiguration : IEntityTypeConfiguration<Produto>
{
    public void Configure(EntityTypeBuilder<Produto> builder)
    {
        // Nome da tabela
        builder.ToTable("Produtos");

        // Chave Primária
        builder.HasKey(p => p.Id);

        // Configurações das colunas
        builder.Property(p => p.Nome)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(p => p.Preco)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(p => p.Descricao)
            .HasMaxLength(500);

        builder.Property(p => p.UrlImagem)
            .HasMaxLength(300);
    }
}