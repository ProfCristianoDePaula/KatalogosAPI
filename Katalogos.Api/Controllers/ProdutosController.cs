using Katalogos.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Katalogos.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProdutosController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProdutosController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // 🟢 Aberto ao público (A vitrine da loja)
    [HttpGet]
    [AllowAnonymous]
    public IActionResult GetProdutos()
    {
        return Ok(new[] { "Notebook", "Teclado Mecânico", "Mouse Gamer" });
    }

    // 🔴 Restrito: Apenas usuários autenticados com o Cargo (Role) de Admin
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CriarProduto([FromBody] CriarProdutoCommand command)
    {
        // O MediatR manda o pedido para a "Cozinha" (Handler) que criamos no Módulo 1
        var produtoId = await _mediator.Send(command);

        return CreatedAtAction(nameof(GetProdutos), new { id = produtoId }, command);
    }
}