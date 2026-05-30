using MediatR;

namespace Katalogos.Application.Commands;

// 1. A COMANDA (O pedido em si)
// IRequest<Guid> significa que, após criar o produto, o sistema vai devolver um ID (Guid)
public class CriarProdutoCommand : IRequest<Guid>
{
    public string Nome { get; set; } = string.Empty;
    public decimal Preco { get; set; }
    public string Descricao { get; set; } = string.Empty;
}

// 2. A COZINHA (O Handler que vai preparar o pedido)
public class CriarProdutoHandler : IRequestHandler<CriarProdutoCommand, Guid>
{
    public async Task<Guid> Handle(CriarProdutoCommand request, CancellationToken cancellationToken)
    {
        // Aqui, nos próximos módulos, vamos injetar o Banco de Dados para salvar de verdade
        Console.WriteLine($"[LOG] Processando a criação do produto: {request.Nome}");

        // Simulando que o produto foi criado e gerando um ID novo
        var novoIdProduto = Guid.NewGuid();

        return await Task.FromResult(novoIdProduto);
    }
}