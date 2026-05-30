using System.Text.Json;
using Katalogos.Application.Interfaces;
using Microsoft.Extensions.Caching.Distributed;

namespace Katalogos.Infrastructure.Services;

public class RedisCacheService : ICacheService
{
    private readonly IDistributedCache _cache;

    public RedisCacheService(IDistributedCache cache)
    {
        _cache = cache;
    }

    public async Task<T?> ObterAsync<T>(string chave)
    {
        var stringCache = await _cache.GetStringAsync(chave);

        if (string.IsNullOrEmpty(stringCache))
            return default; // Cache Miss! (Não achou)

        // Cache Hit! Traduz o JSON de volta para o objeto C#
        return JsonSerializer.Deserialize<T>(stringCache);
    }

    public async Task DefinirAsync<T>(string chave, T valor, TimeSpan tempoExpiracao)
    {
        var opcoes = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = tempoExpiracao // Tempo de vida no cache
        };

        var stringCache = JsonSerializer.Serialize(valor);

        await _cache.SetStringAsync(chave, stringCache, opcoes);
    }
}