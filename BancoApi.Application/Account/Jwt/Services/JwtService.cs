using BancoApi.Application.Account.Jwt.Dtos;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BancoApi.Application.Account.Jwt.Services;
public class JwtService : IJwtService
{
    private readonly IConfiguration _configuration;
    public JwtService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<string> GenerateToken(JwtDto jwtDto)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var token = tokenHandler.CreateToken(GetTokenDescriptor(jwtDto));

        return tokenHandler.WriteToken(token);
    }

    public async Task<JwtTokenViewModel> ReadTokenAsync(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtSecurityToken = handler.ReadJwtToken(token);
        var walletClaim = jwtSecurityToken.Claims.FirstOrDefault(u => u.Type == "walletId")?.Value;

        return await Task.FromResult(
            new JwtTokenViewModel
            {
                Id = Guid.Parse(jwtSecurityToken.Claims.FirstOrDefault(u => u.Type == "sub")?.Value),
                Email = jwtSecurityToken.Claims.FirstOrDefault(u => u.Type == "email")?.Value,
                WalletId = Guid.TryParse(walletClaim, out var walletId) ? walletId : Guid.Empty
            }
        );
    }

    private SecurityTokenDescriptor GetTokenDescriptor(JwtDto jwtDto)
    {
        var key = Encoding.ASCII.GetBytes(_configuration["JwtSecurity:SecurityKey"]);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                    new Claim(ClaimTypes.Email, jwtDto.Email.ToString()),
                    new Claim(JwtRegisteredClaimNames.Sub, jwtDto.Id.ToString()),
                    new Claim("walletId", jwtDto.WalletId.ToString())

            }),
            Expires = DateTime.UtcNow.AddHours(double.Parse(_configuration["JwtSecurity:Expiration"])),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        return tokenDescriptor;
    }
}