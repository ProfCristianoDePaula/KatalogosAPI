namespace Katalogos.Api.Models;

public class LoginUsuarioRequest
{
    public string Email { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;
}