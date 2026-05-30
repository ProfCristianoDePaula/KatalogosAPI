namespace Katalogos.Application.Interfaces;

public interface IProdutoRepository
{
    // O contrato exige que quem for salvar o produto, devolva o ID gerado
    Task<Guid> AdicionarProdutoAsync(string nome, decimal preco, string descricao);
}