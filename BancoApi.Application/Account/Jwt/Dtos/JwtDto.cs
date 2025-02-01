using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancoApi.Application.Account.Jwt.Dtos;
public record JwtDto
{
    public Guid Id { get; private set; }
    public string Email { get; private set; }
    public Guid WalletId { get; private set; }
    public string UserCpf { get; private set; }

    public JwtDto(Guid id,string email, Guid walletId, string userCpf)
    {
        Id = id;
        Email = email;
        WalletId = walletId;
        UserCpf = userCpf;
    }
}
