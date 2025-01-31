using BancoApi.Application.Account.Auth.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancoApi.Application.Account.Auth.Services;
public interface IAuthService
{
    Task<UserResponse> LoginAsync(LoginRequest request);
}