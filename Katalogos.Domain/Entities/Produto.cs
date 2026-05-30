namespace Katalogos.Domain.Entities;

public class Produto
{
    // O 'private set' garante que ninguém de fora pode bagunçar o ID ou o Nome do produto sem usar um construtor
    public Guid Id { get; private set; }
    public string Nome { get; private set; }
    public decimal Preco { get; private set; }
    public string Descricao { get; private set; }
    public string UrlImagem { get; private set; }
    public DateTime DataCadastro { get; private set; }

    // Construtor vazio exigido pelo Entity Framework
    protected Produto() { }

    public Produto(string nome, decimal preco, string descricao, string urlImagem = "")
    {
        Id = Guid.NewGuid();
        Nome = nome;
        Preco = preco;
        Descricao = descricao;
        UrlImagem = urlImagem;
        DataCadastro = DateTime.UtcNow;
    }

    // Método para atualizar a foto depois do upload no Cloudflare
    public void AtualizarImagem(string urlImagem)
    {
        UrlImagem = urlImagem;
    }
}