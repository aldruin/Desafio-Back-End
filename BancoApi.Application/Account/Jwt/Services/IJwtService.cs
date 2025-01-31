using BancoApi.Application.Account.Jwt.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancoApi.Application.Account.Jwt.Services;
public interface IJwtService
{
    Task<string> GenerateToken(JwtDto jwtDto);
    Task<JwtTokenViewModel> ReadTokenAsync(string token);
}