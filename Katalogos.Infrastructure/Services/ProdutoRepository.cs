using Katalogos.Application.Interfaces;
using Katalogos.Domain.Entities;
using Katalogos.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Katalogos.Infrastructure.Services;

public class ProdutoRepository : IProdutoRepository
{
    private readonly KatalogosDbContext _context;

    // Injetamos o banco de dados de verdade aqui!
    public ProdutoRepository(KatalogosDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> AdicionarProdutoAsync(string nome, decimal preco, string descricao)
    {
        var novoProduto = new Produto(nome, preco, descricao);

        await _context.Produtos.AddAsync(novoProduto);
        await _context.SaveChangesAsync(); // Dá o COMMIT no PostgreSQL

        Console.WriteLine($"[BANCO DE DADOS] Produto {nome} salvo com sucesso no PostgreSQL!");
        return novoProduto.Id;
    }

    public async Task<IEnumerable<object>> ObterTodosAtivosAsync()
    {
        // Faz um SELECT * FROM Produtos de forma ultra rápida
        var produtos = await _context.Produtos
            .AsNoTracking() // Dica de Sênior: Deixa a leitura 30% mais rápida!
            .ToListAsync();

        return produtos;
    }
}