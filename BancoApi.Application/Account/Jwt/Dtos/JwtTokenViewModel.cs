using BancoApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancoApi.Application.Account.Jwt.Dtos;
public record JwtTokenViewModel
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public Guid WalletId { get; set; }
}