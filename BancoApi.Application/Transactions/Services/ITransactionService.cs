using BancoApi.Application.Transactions.Dtos;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BancoApi.Application.Transactions.Services;
public interface ITransactionService
{
    Task<TransactionDto> DepositAsync(ClaimsPrincipal user, TransactionDto dto);
    Task<TransactionDto> WithdrawAsync(ClaimsPrincipal user, TransactionDto dto);
    Task<TransactionDto> TransferAsync(ClaimsPrincipal user, TransactionDto dto);
}
