using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancoApi.Application.Account.Auth.Dtos;
public record UserResponse
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string JwtToken { get; set; }
}
