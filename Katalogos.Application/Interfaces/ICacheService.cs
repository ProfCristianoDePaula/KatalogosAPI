namespace Katalogos.Application.Interfaces;

public interface ICacheService
{
    // Tenta buscar o dado. O <T> significa que pode ser qualquer coisa (um Produto, uma Lista, um Usuário)
    Task<T?> ObterAsync<T>(string chave);

    // Salva o dado no cache com um tempo de validade (Ex: expira em 10 minutos)
    Task DefinirAsync<T>(string chave, T valor, TimeSpan tempoExpiracao);
}