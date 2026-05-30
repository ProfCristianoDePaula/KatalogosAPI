using Katalogos.Application.Interfaces;

namespace Katalogos.Infrastructure.Services;

// A classe DEVE ser public para o Program.cs enxergá-la
public class ProdutoRepository : IProdutoRepository
{
    public async Task<Guid> AdicionarProdutoAsync(string nome, decimal preco, string descricao)
    {
        Console.WriteLine($"[BANCO DE DADOS] Salvando o produto {nome} no valor de R$ {preco}...");
        return await Task.FromResult(Guid.NewGuid());
    }
}