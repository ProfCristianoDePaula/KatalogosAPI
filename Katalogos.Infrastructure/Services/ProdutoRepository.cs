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

    public async Task<IEnumerable<object>> ObterTodosAtivosAsync()
    {
        // Simulando uma ida demorada ao banco de dados (2 segundos de lentidão)
        await Task.Delay(2000);

        return new List<object>
    {
        new { Id = Guid.NewGuid(), Nome = "Teclado Mecânico RGB", Preco = 350.00 },
        new { Id = Guid.NewGuid(), Nome = "Mouse Gamer Ultra", Preco = 120.00 }
    };
    }
}