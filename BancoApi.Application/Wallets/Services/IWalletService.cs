using BancoApi.Application.Wallets.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancoApi.Application.Wallets.Services;
public interface IWalletService
{
    Task<WalletDto> CreateAsync(Guid userId);
}
