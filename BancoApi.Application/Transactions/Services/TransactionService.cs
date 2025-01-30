using BancoApi.Application.Notifications;
using BancoApi.Application.Transactions.Dtos;
using BancoApi.Application.Users.Services;
using BancoApi.Application.Validators;
using BancoApi.Application.Wallets.Services;
using BancoApi.Domain.Entities;
using BancoApi.Domain.Enums;
using BancoApi.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace BancoApi.Application.Transactions.Services;
public class TransactionService : ITransactionService
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IWalletService _walletService;
    private readonly INotificationHandler _notificationHandler;
    private readonly IUserService _userService;

    public TransactionService(ITransactionRepository transactionRepository, IWalletService walletService, INotificationHandler notificationHandler, IUserService userService)
    {
        _transactionRepository = transactionRepository;
        _walletService = walletService;
        _notificationHandler = notificationHandler;
        _userService = userService;
    }

    public async Task<TransactionDto> DepositAsync(ClaimsPrincipal user, TransactionDto dto)
    {
        using (var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            try
            {
                var validate = await new TransactionValidator().ValidateAsync(dto);
                if (!validate.IsValid)
                {
                    foreach (var error in validate.Errors)
                    {
                        _notificationHandler.AddNotification("InvalidTransaction", $"Erro ao criar transação: {error.ErrorMessage}");
                        return null;
                    }
                }

                //aqui a ideia é obter a originWallet como wallet do usuario logado, garantindo que
                //a wallet sempre seja referenciada na operação como a do usuario autenticado

                //var destineWallet = Guid.TryParse(user.FindFirst("walletId")?.Value, out var walletId) ? walletId : Guid.Empty;
                //if (destineWallet == Guid.Empty)
                //{
                //    _notificationHandler.AddNotification("InvalidWallet", "Wallet ID do usuário não encontrado ou inválido.");
                //    return null;
                //}

                var authenticatedUserWalletId = dto.DestinationWalletId;

                var operation = TransactionOperation.Deposit;

                var transaction = new TransactionWallet
                {
                    OriginWalletId = authenticatedUserWalletId,
                    DestinationWalletId = dto.DestinationWalletId,
                    Value = dto.Value,
                    TransactionDate = DateTime.UtcNow,
                    Operation = operation
                };

                await _transactionRepository.AddAsync(transaction);

                bool isBalanceUpdated = await _walletService.UpdateBalanceAsync(transaction.Id, transaction.OriginWalletId, transaction.DestinationWalletId, transaction.Value, operation.ToString());
                if (!isBalanceUpdated)
                {
                    _notificationHandler.AddNotification("DepositFailed", "Falha ao atualizar o saldo da carteira de destino.");
                    return null;
                }

                transactionScope.Complete();

                _notificationHandler.AddNotification("TransactionOK", "Transação realizada com sucesso!");
                _notificationHandler.AddNotification("WalletBalanceUpdated", "Deposito realizado com sucesso!");

                return new TransactionDto
                {
                    Id = transaction.Id,
                    OriginWalletId = transaction.OriginWalletId,
                    DestinationWalletId = transaction.DestinationWalletId,
                    Value = transaction.Value,
                    TransactionDate = transaction.TransactionDate
                };
            }
            catch (Exception ex)
            {
                _notificationHandler.AddNotification("DepositTransactionFailed", $"Erro ao realizar transação de deposito: {ex.Message}");
                return null;
            }
        }
    }

    public Task<TransactionDto> TransferAsync(ClaimsPrincipal user, TransactionDto dto)
    {
        throw new NotImplementedException();
    }

    public Task<TransactionDto> WithdrawAsync(ClaimsPrincipal user, TransactionDto dto)
    {
        throw new NotImplementedException();
    }
}
