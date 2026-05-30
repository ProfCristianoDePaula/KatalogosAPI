using Microsoft.AspNetCore.Identity;

namespace Katalogos.Infrastructure.Identity;

// Herdamos do IdentityUser para ganhar e-mail, senha (criptografada) e controle de bloqueio de graça!
// E aqui embaixo, nós adicionamos os campos obrigatórios do nosso e-commerce (Full Identity)
public class KatalogosUser : IdentityUser
{
    public string NomeCompleto { get; set; } = string.Empty;
    public string DocumentoCpf { get; set; } = string.Empty;
    public DateTime DataNascimento { get; set; }

    // O telefone já existe na classe pai (PhoneNumber), mas o endereço precisamos criar:
    public string EnderecoCompleto { get; set; } = string.Empty;

    // Controle de auditoria (Quando ele virou nosso cliente?)
    public DateTime DataRegistro { get; set; } = DateTime.UtcNow;
}