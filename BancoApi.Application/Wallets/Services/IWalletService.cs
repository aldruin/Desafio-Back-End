using BancoApi.Application.Transactions.Dtos;
using BancoApi.Application.Wallets.Dtos;
using BancoApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancoApi.Application.Wallets.Services;
public interface IWalletService
{
    Task<WalletDto> CreateAsync(Guid userId);
    Task<WalletDto> GetByUserIdAsync(Guid userId);
    Task<WalletDto> GetByUserCpfAsync(string userCpf);
    Task<WalletDto> GetByUserEmailAsync(string userEmail);
    Task<bool> UpdateBalanceAsync(Guid transactionId, Guid originWalletId, Guid destinationWalletId, decimal value, string operation);
}
