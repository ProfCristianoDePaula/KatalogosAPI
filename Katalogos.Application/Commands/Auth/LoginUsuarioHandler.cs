using Katalogos.Application.Interfaces;
using MediatR;

namespace Katalogos.Application.Commands.Auth;

public class LoginUsuarioHandler : IRequestHandler<LoginUsuarioCommand, string?>
{
    private readonly IAuthService _authService;

    public LoginUsuarioHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<string?> Handle(LoginUsuarioCommand request, CancellationToken cancellationToken)
    {
        // Chama a infraestrutura para verificar banco e gerar o JWT
        return await _authService.LoginAsync(request.Email, request.Senha);
    }
}