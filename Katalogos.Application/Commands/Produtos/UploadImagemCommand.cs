using Katalogos.Application.Interfaces;
using MediatR;

namespace Katalogos.Application.Commands.Produtos;

// 1. O Pedido
public class UploadImagemCommand : IRequest<string>
{
    public Stream ArquivoStream { get; set; } = Stream.Null;
    public string NomeArquivo { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
}

// 2. A Cozinha
public class UploadImagemHandler : IRequestHandler<UploadImagemCommand, string>
{
    private readonly IStorageService _storageService;

    public UploadImagemHandler(IStorageService storageService)
    {
        _storageService = storageService;
    }

    public async Task<string> Handle(UploadImagemCommand request, CancellationToken cancellationToken)
    {
        // Manda o arquivo pra nuvem e pega a URL de volta
        return await _storageService.UploadArquivoAsync(request.ArquivoStream, request.NomeArquivo, request.ContentType);
    }
}