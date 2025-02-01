using BancoApi.Application.Account.Auth.Dtos;
using BancoApi.Application.Account.Jwt.Dtos;
using BancoApi.Application.Account.Jwt.Services;
using BancoApi.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancoApi.Application.Account.Auth.Services;
public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtService;

    public AuthService(IUserRepository userRepository, IJwtService jwtService)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
    }

    public async Task<UserResponse> LoginAsync(LoginRequest request)
    {
        var user = await _userRepository.GetByExpressionAsync(x => x.Email.Value == request.Email);
        if (user == null) { return null; }
        var jwtToken = await _jwtService.GenerateToken(new JwtDto(user.Id, user.Email.Value, user.Wallet.Id, user.Cpf));

        return new UserResponse
        {
            Id = user.Id,
            Email = user.Email.Value,
            JwtToken = jwtToken,
        };
    }
}