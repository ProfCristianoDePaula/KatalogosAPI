using Katalogos.Api.Models;
using Katalogos.Application.Commands;
using Katalogos.Application.Commands.Produtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Katalogos.Api.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class ProdutosController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProdutosController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // 🟢 Aberto ao público: Qualquer pessoa pode ver a vitrine da Katalogos
    [HttpGet]
    [AllowAnonymous]
    public IActionResult GetProdutos()
    {
        // Por enquanto retornamos dados falsos. No Módulo 5 vamos ligar isso no Redis!
        return Ok(new[] {
            new { Nome = "Teclado Mecânico", Preco = 350.00 },
            new { Nome = "Mouse Gamer", Preco = 120.00 }
        });
    }

    // 🔴 Restrito: Só o Admin com a Pulseira VIP (Token JWT) pode forjar um produto novo
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CriarProduto([FromBody] CriarProdutoRequest request)
    {
        // 1. Traduz da Web (Request) para o Negócio (Command)
        var command = new CriarProdutoCommand
        {
            Nome = request.Nome,
            Preco = request.Preco,
            Descricao = request.Descricao
        };

        // 2. Manda pro garçom (MediatR) levar lá pra camada de Aplicação
        var produtoId = await _mediator.Send(command);

        return Ok(new { Mensagem = "Produto forjado com sucesso na Katalogos!", Id = produtoId });
    }
    // 🔵 Upload de Imagem do Produto
    [HttpPost("upload-imagem")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UploadImagem(IFormFile arquivo)
    {
        if (arquivo == null || arquivo.Length == 0)
            return BadRequest(new { Mensagem = "Nenhum arquivo enviado." });

        // Abre o fluxo de bytes do arquivo
        using var stream = arquivo.OpenReadStream();

        var command = new UploadImagemCommand
        {
            ArquivoStream = stream,
            NomeArquivo = arquivo.FileName,
            ContentType = arquivo.ContentType
        };

        // Manda pro garçom trabalhar
        var urlImagem = await _mediator.Send(command);

        return Ok(new
        {
            Mensagem = "Imagem enviada para a Edge Network com sucesso!",
            Url = urlImagem
        });
    }
}