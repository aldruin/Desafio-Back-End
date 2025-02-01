using BancoApi.Application.Users.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BancoApi.Application.Users.Services;
public interface IUserService
{
    Task<UserDto> CreateAsync(UserDto dto);
    Task<UserDto> GetByIdAsync(ClaimsPrincipal user);
    Task<UserDto> GetByCpfAsync(ClaimsPrincipal user);
    Task<UserDto> GetByEmailAsync(ClaimsPrincipal user);
    Task<UserDto> UpdateAsync(ClaimsPrincipal user, UserDto dto);
    Task<UserDto> RemoveByIdAsync(ClaimsPrincipal user);
}
