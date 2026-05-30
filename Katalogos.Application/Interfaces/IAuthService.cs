namespace Katalogos.Application.Interfaces;

// O Contrato: A Aplicação não sabe COMO vai ser feito, só sabe O QUE precisa ser feito.
public interface IAuthService
{
    Task<bool> RegistrarUsuarioAsync(string email, string senha, string nomeCompleto, string cpf);
    Task<string?> LoginAsync(string email, string senha);
}