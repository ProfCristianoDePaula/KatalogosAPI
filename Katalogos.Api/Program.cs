using Asp.Versioning;
using Katalogos.Application.Commands.Auth;
using Katalogos.Application.Interfaces;
using Katalogos.Infrastructure.Data;
using Katalogos.Infrastructure.Identity;
using Katalogos.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

// 👇 AS DUAS NOVAS BIBLIOTECAS DO .NET 10 (Esqueça o .Models!)
using Microsoft.OpenApi;
using Scalar.AspNetCore;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// 1. Adicionando suporte aos Controllers
builder.Services.AddControllers();

//// 2. Conectando nosso Banco de Dados
//builder.Services.AddDbContext<KatalogosDbContext>(options =>
//    options.UseInMemoryDatabase("KatalogosDbLocal"));

// 2. Conectando nosso Banco de Dados de VERDADE (PostgreSQL no Docker)
var stringDeConexao = "Host=localhost;Port=5432;Database=katalogos_db;Username=admin;Password=rootpassword123";

builder.Services.AddDbContext<KatalogosDbContext>(options =>
    options.UseNpgsql(stringDeConexao));

// Configurando o Redis (A porta 6379 é a que mapeamos no nosso docker-compose!)
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost:6379";
});

// Assinando a carteira de trabalho do Cache
builder.Services.AddScoped<ICacheService, RedisCacheService>();

// 3. Adicionando o nosso Full Identity
builder.Services.AddIdentity<KatalogosUser, IdentityRole>()
    .AddEntityFrameworkStores<KatalogosDbContext>()
    .AddDefaultTokenProviders();

// 4. Configurando a segurança JWT
var jwtSecret = builder.Configuration["JwtSettings:Secret"] ?? "ChaveSuperSecretaDeDesenvolvimentoKatalogos2026!!";
var chaveSecreta = Encoding.ASCII.GetBytes(jwtSecret);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(chaveSecreta),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

// Injeção de Dependências
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();
builder.Services.AddScoped<IStorageService, CloudflareStorageService>();

// Configurando o Garçom (MediatR)
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(RegistrarUsuarioCommand).Assembly));

// 5. Configurando o Versionamento da API
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
}).AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

// 6. Gerador de Documentação OpenAPI (PADRÃO .NET 10)
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Components ??= new OpenApiComponents();
        document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();

        // Define o esquema do cadeado (Sem usar "Reference" aqui)
        document.Components.SecuritySchemes.Add("Bearer", new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            In = ParameterLocation.Header,
            BearerFormat = "Json Web Token",
            Description = "Cole seu Token JWT"
        });

        // Agora a propriedade se chama Security (e não SecurityRequirements)
        document.Security ??= new List<OpenApiSecurityRequirement>();

        // No .NET 10, a chave do dicionário PRECISA ser um OpenApiSecuritySchemeReference
        document.Security.Add(new OpenApiSecurityRequirement
        {
            [new OpenApiSecuritySchemeReference("Bearer", document)] = new List<string>()
        });

        return Task.CompletedTask;
    });
});

// ==========================================
// PIPELINE DE EXECUÇÃO
// ==========================================
var app = builder.Build();

// 7. Interface do Scalar
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("Katalogos API - Catálogo Master");
        options.WithTheme(ScalarTheme.DeepSpace);
        options.WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
    });
}

// 8. Segurança e Roteamento
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();