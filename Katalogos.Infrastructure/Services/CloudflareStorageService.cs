using Amazon.S3;
using Amazon.S3.Transfer;
using Katalogos.Application.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Katalogos.Infrastructure.Services;

public class CloudflareStorageService : IStorageService
{
    private readonly string _bucketName;
    private readonly string _publicUrl;
    private readonly IAmazonS3 _s3Client;

    public CloudflareStorageService(IConfiguration configuration)
    {
        // Pegamos os dados mágicos do appsettings.json
        _bucketName = configuration["CloudflareR2:BucketName"]!;
        _publicUrl = configuration["CloudflareR2:PublicUrl"]!;

        var accessKey = configuration["CloudflareR2:AccessKey"];
        var secretKey = configuration["CloudflareR2:SecretKey"];
        var accountId = configuration["CloudflareR2:AccountId"];

        // Configura o motorista do S3 para apontar para a Cloudflare em vez da AWS
        var s3Config = new AmazonS3Config
        {
            ServiceURL = $"https://{accountId}.r2.cloudflarestorage.com",
        };

        _s3Client = new AmazonS3Client(accessKey, secretKey, s3Config);
    }

    public async Task<string> UploadArquivoAsync(Stream arquivo, string nomeArquivo, string contentType)
    {
        // Monta um nome único para não sobrepor arquivos com o mesmo nome
        var nomeUnico = $"{Guid.NewGuid()}-{nomeArquivo}";

        var uploadRequest = new TransferUtilityUploadRequest
        {
            InputStream = arquivo,
            Key = nomeUnico,
            BucketName = _bucketName,
            ContentType = contentType,
            DisablePayloadSigning = true // Exigência do Cloudflare R2
        };

        var transferUtility = new TransferUtility(_s3Client);
        await transferUtility.UploadAsync(uploadRequest);

        // Retorna o link final pronto para ser salvo no Banco de Dados
        return $"{_publicUrl}/{nomeUnico}";
    }
}