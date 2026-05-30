using Katalogos.Application.Interfaces;
using MediatR;

namespace Katalogos.Application.Commands.Produtos;

// 1. O PEDIDO (Command)
public class CriarProdutoCommand : IRequest<Guid>
{
    public string Nome { get; set; } = string.Empty;
    public decimal Preco { get; set; }
    public string Descricao { get; set; } = string.Empty;
}

// 2. A COZINHA (Handler)
public class CriarProdutoHandler : IRequestHandler<CriarProdutoCommand, Guid>
{
    private readonly IProdutoRepository _produtoRepository;

    // A cozinha confia apenas no Contrato (Interface). Ela não sabe o que é banco de dados!
    public CriarProdutoHandler(IProdutoRepository produtoRepository)
    {
        _produtoRepository = produtoRepository;
    }

    public async Task<Guid> Handle(CriarProdutoCommand request, CancellationToken cancellationToken)
    {
        // Chama a Infraestrutura para fazer o trabalho sujo de salvar de verdade
        return await _produtoRepository.AdicionarProdutoAsync(request.Nome, request.Preco, request.Descricao);
    }
}