using MediatR;

namespace Katalogos.Application.Commands.Auth;

public class LoginUsuarioCommand : IRequest<string?>
{
    public string Email { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;
}