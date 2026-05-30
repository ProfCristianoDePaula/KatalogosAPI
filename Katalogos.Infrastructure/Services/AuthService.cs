using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Katalogos.Application.Commands.Auth;
using Katalogos.Application.Interfaces;
using Katalogos.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Katalogos.Infrastructure.Services;

// A Infraestrutura ASSINA o contrato da Application (IAuthService)
public class AuthService : IAuthService
{
    private readonly UserManager<KatalogosUser> _userManager;
    private readonly IConfiguration _configuration;

    public AuthService(UserManager<KatalogosUser> userManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _configuration = configuration;
    }

    public async Task<bool> RegistrarUsuarioAsync(string email, string senha, string nomeCompleto, string cpf)
    {
        var novoUsuario = new KatalogosUser
        {
            UserName = email,
            Email = email,
            NomeCompleto = nomeCompleto,
            DocumentoCpf = cpf,
            DataRegistro = DateTime.UtcNow
        };

        var resultado = await _userManager.CreateAsync(novoUsuario, senha);
        return resultado.Succeeded;
    }

    public async Task<string?> LoginAsync(string email, string senha)
    {
        var usuario = await _userManager.FindByEmailAsync(email);

        if (usuario != null && await _userManager.CheckPasswordAsync(usuario, senha))
        {
            return GerarTokenJwt(usuario);
        }

        return null; // Login falhou
    }

    private string GerarTokenJwt(KatalogosUser usuario)
    {
        var jwtSecret = _configuration["JwtSettings:Secret"] ?? "ChaveSuperSecretaDeDesenvolvimentoKatalogos2026!!";
        var chave = Encoding.ASCII.GetBytes(jwtSecret);

        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id),
                new Claim(ClaimTypes.Email, usuario.Email!),
                new Claim(ClaimTypes.Name, usuario.NomeCompleto),
                new Claim(ClaimTypes.Role, "Admin")
            }),
            Expires = DateTime.UtcNow.AddHours(2),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(chave), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}