using System.Text;
using Asp.Versioning;
using Katalogos.Infrastructure.Data;
using Katalogos.Infrastructure.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Adicionando suporte aos Controllers
builder.Services.AddControllers();

// 2. Conectando nosso Banco de Dados (Usando em memória para os testes locais rodarem lisos)
builder.Services.AddDbContext<KatalogosDbContext>(options =>
    options.UseInMemoryDatabase("KatalogosDbLocal"));

// 3. Adicionando o nosso Full Identity customizado
builder.Services.AddIdentity<KatalogosUser, IdentityRole>()
    .AddEntityFrameworkStores<KatalogosDbContext>()
    .AddDefaultTokenProviders();

// 4. Configurando a segurança JWT (A Pulseira VIP)
// Truque de Desenvolvimento: Tenta pegar a chave do appsettings. Se não achar, usa a string de teste para não quebrar a aplicação!
var jwtSecret = builder.Configuration["JwtSettings:Secret"] ?? "ChaveSuperSecretaDeDesenvolvimentoKatalogos2026!!";
var chaveSecreta = Encoding.ASCII.GetBytes(jwtSecret);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false; // Permite rodar sem HTTPS localmente
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(chaveSecreta),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

// 5. Configurando o Versionamento da API (Profissionalismo: v1.0)
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

// 6. Gerador de Documentação OpenAPI do próprio .NET 10
builder.Services.AddOpenApi();

// ==========================================
// PIPELINE DE EXECUÇÃO (Middlewares)
// ==========================================
var app = builder.Build();

// 7. Interface Hacker/Moderna do Scalar (Só roda em ambiente de Dev)
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi(); // Gera o mapeamento JSON dos endpoints
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("Katalogos API - Catálogo Master");
        options.WithTheme(ScalarTheme.DeepSpace); // Um tema escuro espetacular para a documentação
        options.WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
    });
}

// 8. Segurança e Roteamento
app.UseHttpsRedirection();

// A ORDEM AQUI É SAGRADA! 
// Primeiro você Autentica (Descobre quem é o usuário)
app.UseAuthentication();
// Depois você Autoriza (Deixa ele entrar na rota ou não)
app.UseAuthorization();

// 9. Mapeando os Endpoints dos Controllers
app.MapControllers();

// 10. Start na Aplicação!
app.Run();