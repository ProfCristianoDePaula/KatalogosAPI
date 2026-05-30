using Katalogos.Api.Models;
using Katalogos.Application.Commands.Auth;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Katalogos.Api.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("registrar")]
    [AllowAnonymous] // Qualquer pessoa da internet pode tentar se registrar
    public async Task<IActionResult> Registrar([FromBody] RegistroUsuarioRequest request)
    {
        var command = new RegistrarUsuarioCommand
        {
            Email = request.Email,
            Senha = request.Senha,
            NomeCompleto = request.NomeCompleto,
            DocumentoCpf = request.DocumentoCpf
        };

        var sucesso = await _mediator.Send(command);

        if (sucesso)
            return Ok(new { Mensagem = "Usuário criado com sucesso!" });

        return BadRequest(new { Mensagem = "Erro ao criar usuário. Verifique as regras de senha e e-mail." });
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginUsuarioRequest request)
    {
        var command = new LoginUsuarioCommand
        {
            Email = request.Email,
            Senha = request.Senha
        };

        var token = await _mediator.Send(command);

        if (!string.IsNullOrEmpty(token))
        {
            return Ok(new
            {
                Mensagem = "Login realizado com sucesso! Copie seu Token.",
                Token = token
            });
        }

        return Unauthorized(new { Mensagem = "E-mail ou senha incorretos." });
    }
}