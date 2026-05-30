namespace Katalogos.Api.Models;

// Isso é o que vem do celular do usuário (O JSON)
public class RegistroUsuarioRequest
{
    public string Email { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;
    public string NomeCompleto { get; set; } = string.Empty;
    public string DocumentoCpf { get; set; } = string.Empty;
}