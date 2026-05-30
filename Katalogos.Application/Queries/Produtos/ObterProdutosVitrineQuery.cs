using Katalogos.Application.Interfaces;
using MediatR;

namespace Katalogos.Application.Queries.Produtos;

// 1. A PERGUNTA (Query)
public class ObterProdutosVitrineQuery : IRequest<IEnumerable<object>>
{
    // Não precisa de propriedades, o cliente só quer "Ver Tudo"
}

// 2. A RESPOSTA (Handler aplicando o Cache-Aside)
public class ObterProdutosVitrineHandler : IRequestHandler<ObterProdutosVitrineQuery, IEnumerable<object>>
{
    private readonly IProdutoRepository _produtoRepository;
    private readonly ICacheService _cacheService;

    public ObterProdutosVitrineHandler(IProdutoRepository produtoRepository, ICacheService cacheService)
    {
        _produtoRepository = produtoRepository;
        _cacheService = cacheService;
    }

    public async Task<IEnumerable<object>> Handle(ObterProdutosVitrineQuery request, CancellationToken cancellationToken)
    {
        var chaveCache = "vitrine_produtos_ativos";

        // PASSO 1: O Garçom olha no bloquinho (Redis)
        var produtosNoCache = await _cacheService.ObterAsync<IEnumerable<object>>(chaveCache);

        if (produtosNoCache != null)
        {
            Console.WriteLine("[⚡ REDIS] Cache HIT! Retornando da memória em 1ms!");
            return produtosNoCache;
        }

        // PASSO 2: Não achou. Vai até a cozinha (PostgreSQL) - Vai demorar 2 segundos!
        Console.WriteLine("[🐢 POSTGRES] Cache MISS! Indo buscar no banco de dados lento...");
        var produtosDoBanco = await _produtoRepository.ObterTodosAtivosAsync();

        // PASSO 3: Anota no bloquinho para a próxima vez (Expira em 1 minuto)
        await _cacheService.DefinirAsync(chaveCache, produtosDoBanco, TimeSpan.FromMinutes(1));

        return produtosDoBanco;
    }
}