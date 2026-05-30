using Katalogos.Application.Interfaces;
using MediatR;

namespace Katalogos.Application.Commands.Auth;

// 1. O Pedido (Command)
public class RegistrarUsuarioCommand : IRequest<bool>
{
    public string Email { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;
    public string NomeCompleto { get; set; } = string.Empty;
    public string DocumentoCpf { get; set; } = string.Empty;
}

// 2. A Cozinha (Handler)
public class RegistrarUsuarioHandler : IRequestHandler<RegistrarUsuarioCommand, bool>
{
    private readonly IAuthService _authService;

    // Injetamos o contrato. O Handler não sabe que é o Identity que tá rodando por baixo dos panos!
    public RegistrarUsuarioHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<bool> Handle(RegistrarUsuarioCommand request, CancellationToken cancellationToken)
    {
        // Aqui chamamos o serviço que vai fazer o trabalho sujo de ir no banco
        return await _authService.RegistrarUsuarioAsync(
            request.Email,
            request.Senha,
            request.NomeCompleto,
            request.DocumentoCpf);
    }
}