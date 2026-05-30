namespace Katalogos.Application.Interfaces;

public interface IStorageService
{
    // Recebe o arquivo em formato de "Stream" (fluxo de bytes), o nome e o tipo (ex: image/png)
    // Retorna a URL pública da imagem na internet
    Task<string> UploadArquivoAsync(Stream arquivo, string nomeArquivo, string contentType);
}