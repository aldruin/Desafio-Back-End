using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancoApi.Application.Account.Auth.Dtos;
public record LoginRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
}