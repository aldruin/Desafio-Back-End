using BancoApi.Application.Notifications;
using BancoApi.Application.Transactions.Dtos;
using BancoApi.Application.Users.Services;
using BancoApi.Application.Validators;
using BancoApi.Application.Wallets.Services;
using BancoApi.Domain.Entities;
using BancoApi.Domain.Enums;
using BancoApi.Domain.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
                if(dto.Value <= 0)
                {
                    _notificationHandler.AddNotification("InvalidTransaction", "Erro ao criar transação, Valor deve ser maior do que zero.");
                    return null;
                }


                var loggerUserWallet = Guid.TryParse(user.FindFirst("walletId")?.Value, out var walletId) ? walletId : Guid.Empty;
                if (loggerUserWallet == Guid.Empty)
                {
                    _notificationHandler.AddNotification("InvalidWallet", "Wallet ID do usuário não encontrado ou inválido.");
                    return null;
                }

                var authenticatedUserWalletId = loggerUserWallet;

                var operation = TransactionOperation.Deposit;

                var transaction = new TransactionWallet
                {
                    OriginWalletId = authenticatedUserWalletId,
                    DestinationWalletId = authenticatedUserWalletId,
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
 
    public async Task<List<TransactionDto>> GetTransactionsByUserWithOptionalDateFilter(ClaimsPrincipal user, DateTime? date)
    {
        var loggedUserId = Guid.TryParse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var userId) ? userId : Guid.Empty;
        if(loggedUserId == Guid.Empty)
        {
            _notificationHandler.AddNotification("InvalidUserId", "Não foi possivel obter user id do usuário logado");
        }

        var loggedUserWallet = Guid.TryParse(user.FindFirst("walletId")?.Value, out var walletId) ? walletId : Guid.Empty;
        if (loggedUserWallet == Guid.Empty)
        {
            _notificationHandler.AddNotification("InvalidUserWallet", "Não foi possivel obter carteira do usuário logado");
        }

        DateTime? adjustedDate = date.HasValue 
            ? DateTime.SpecifyKind(date.Value, DateTimeKind.Utc) 
            : (DateTime?)null;

        var transactions = adjustedDate.HasValue
            ? await _transactionRepository.GetListByExpressionAsync(t => t.OriginWalletId == loggedUserWallet && t.TransactionDate.Date == adjustedDate.Value.Date) 
            : await _transactionRepository.GetListByExpressionAsync(t => t.OriginWalletId == loggedUserWallet);

        if (transactions == null || !transactions.Any())
        {
            _notificationHandler.AddNotification("NoTransactions", "Nenhuma transação encontrada para os critérios especificados.");
            return new List<TransactionDto>();
        }

        _notificationHandler.AddNotification("Success", "Transações obtidas com sucesso.");

        return transactions.Select(t => new TransactionDto
        {
            Id = t.Id,
            OriginWalletId = t.OriginWalletId,
            DestinationWalletId = t.DestinationWalletId,
            Value = t.Value,
            TransactionDate = t.TransactionDate
        }).ToList();
    }

    public async Task<TransactionDto> TransferAsync(ClaimsPrincipal user, TransactionDto dto)
    {
        using (var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            try
            {
                if (dto.Value == null || dto.Value <= 0)
                {
                    _notificationHandler.AddNotification("ValueMustBeValuable", "O valor não pode ser nulo. Valor precisa ser maior do que zero.");
                    return null;
                }

                if (string.IsNullOrWhiteSpace(dto.Cpf) ||
                    !System.Text.RegularExpressions.Regex.IsMatch(dto.Cpf, @"^\d{11}$|^\d{3}\.\d{3}\.\d{3}-\d{2}$"))
                {
                    _notificationHandler.AddNotification("CpfInvalido", "O CPF é obrigatório e deve estar no formato 000.000.000-00 ou 00000000000.");
                    return null;
                }

                var loggedUserWallet = Guid.TryParse(user.FindFirst("walletId")?.Value, out var walletId) ? walletId : Guid.Empty;
                if (loggedUserWallet == Guid.Empty)
                {
                    _notificationHandler.AddNotification("InvalidWallet", "Wallet ID de origem não encontrada.");
                    return null;
                }

                var destinationWallet = await _walletService.GetWalletByCpf(dto.Cpf);

                Guid destinationWalletId = (destinationWallet?.Id.HasValue == true && Guid.TryParse(destinationWallet.Id.Value.ToString(), out var destinationId))
                    ? destinationId
                    : Guid.Empty;

                var operation = TransactionOperation.Transference;
                var transaction = new TransactionWallet
                {
                    OriginWalletId = loggedUserWallet,
                    DestinationWalletId = destinationWalletId,
                    Value = dto.Value,
                    TransactionDate = DateTime.UtcNow,
                    Operation = operation
                };

                await _transactionRepository.AddAsync(transaction);

                bool isTransfered = await _walletService.UpdateBalanceAsync(transaction.Id, transaction.OriginWalletId, transaction.DestinationWalletId, transaction.Value, operation.ToString());
                if (!isTransfered)
                {
                    _notificationHandler.AddNotification("TransferenceFailed", "Falha ao atualizar o saldo da carteira de destino.");
                    return null;
                }

                transactionScope.Complete();

                _notificationHandler.AddNotification("TransactionOK", "Transação realizada com sucesso!");
                _notificationHandler.AddNotification("WalletBalanceUpdated", "Transferencia realizada com sucesso!");

                return new TransactionDto
                {
                    Id = transaction.Id,
                    OriginWalletId = transaction.OriginWalletId,
                    DestinationWalletId = transaction.DestinationWalletId,
                    Value = transaction.Value,
                    TransactionDate = transaction.TransactionDate
                };
            }
            catch(Exception ex)
            {
                _notificationHandler.AddNotification("TransferenceFailed", $"Erro ao realizar transferencia: {ex.Message}");
                return null;
            }
        }
    }

    public async Task<TransactionDto> WithdrawAsync(ClaimsPrincipal user, TransactionDto dto)
    {
        throw new NotImplementedException();
    }
}
