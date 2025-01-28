using BancoApi.Application.Users.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancoApi.Application.Users.Services;
public interface IUserService
{
    Task<UserDto> CreateAsync(UserDto dto);
    Task<UserDto> GetByIdAsync(Guid id);
    Task<UserDto> GetByCpfAsync(string cpf);
    Task<UserDto> GetByEmailAsync(string email);
    Task<UserDto> UpdateAsync(UserDto dto);
    Task<UserDto> RemoveByIdAsync(Guid id);
}
