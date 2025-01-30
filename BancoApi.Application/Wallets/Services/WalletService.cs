using BancoApi.Application.Notifications;
using BancoApi.Application.Transactions.Dtos;
using BancoApi.Application.Transactions.Services;
using BancoApi.Application.Validators;
using BancoApi.Application.Wallets.Dtos;
using BancoApi.Domain.Entities;
using BancoApi.Domain.Enums;
using BancoApi.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BancoApi.Application.Wallets.Services;
public class WalletService : IWalletService
{
    private readonly IWalletRepository _walletRepository;
    private readonly INotificationHandler _notificationHandler;

    public WalletService(IWalletRepository walletRepository, INotificationHandler notificationHandler)
    {
        _walletRepository = walletRepository;
        _notificationHandler = notificationHandler;
    }

    public async Task<WalletDto> CreateAsync(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            _notificationHandler.AddNotification("FailToCreateWallet", "Erro ao criar Carteira de Usuario, userId invalido, erro interno no servidor.");
            return null;
        }

        var wallet = new Wallet
        {
            UserId = userId,
            Balance = 0
        };

        await _walletRepository.AddAsync(wallet);

        _notificationHandler.AddNotification("WalletCreated", "Carteira de usuário criada com sucesso!");
        return new WalletDto
        {
            Id = wallet.Id,
            UserId = wallet.UserId,
            Balance = wallet.Balance
        };

    }

    public async Task<WalletDto> GetByUserIdAsync(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            _notificationHandler.AddNotification("FailToGetWallet", "Erro ao obter carteira de usuário, informe um ID válido do tipo Guid.");
            return null;
        }

        var wallet = await _walletRepository.GetByExpressionAsync(w => w.UserId == userId);

        if (wallet == null)
        {
            _notificationHandler.AddNotification("WalletNotFound", "Nenhuma carteira encontrada com ID de usuário fornecido.");
            return null;
        }

        _notificationHandler.AddNotification("WalletFound", "Carteira de usuário obtida com sucesso!");
        return new WalletDto
        {
            Id = wallet.Id,
            UserId = wallet.UserId,
            Balance = wallet.Balance,
            TransactionsId = wallet.TransactionsId
        };
    }

    public async Task<WalletDto> GetByUserCpfAsync(string userCpf)
    {
        if (userCpf == null)
        {
            _notificationHandler.AddNotification("FailToGetWallet", "Erro ao obter carteira de usuário, informe um CPF válido.");
            return null;
        }

        var wallet = await _walletRepository.GetByExpressionAsync(w => w.User.Cpf == userCpf);

        if (wallet == null)
        {
            _notificationHandler.AddNotification("WalletNotFound", "Nenhuma carteira encontrada com CPF de usuário fornecido.");
            return null;
        }

        _notificationHandler.AddNotification("WalletFound", "Carteira de usuário obtida com sucesso!");
        return new WalletDto
        {
            Id = wallet.Id,
            UserId = wallet.UserId,
            Balance = wallet.Balance,
            TransactionsId = wallet.TransactionsId
        };
    }

    public async Task<WalletDto> GetByUserEmailAsync(string userEmail)
    {
        {
            if (userEmail == null)
            {
                _notificationHandler.AddNotification("FailToGetWallet", "Erro ao obter carteira de usuário, informe um Email válido.");
                return null;
            }

            var wallet = await _walletRepository.GetByExpressionAsync(w => w.User.Email.Value == userEmail);

            if (wallet == null)
            {
                _notificationHandler.AddNotification("WalletNotFound", "Nenhuma carteira encontrada com Email de usuário fornecido.");
                return null;
            }

            _notificationHandler.AddNotification("WalletFound", "Carteira de usuário obtida com sucesso!");
            return new WalletDto
            {
                Id = wallet.Id,
                UserId = wallet.UserId,
                Balance = wallet.Balance,
                TransactionsId = wallet.TransactionsId
            };
        }
    }

    public async Task<bool> UpdateBalanceAsync(Guid transactionId, Guid originWalletId, Guid destinationWalletId, decimal value, string operation)
    {
        using (var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            try
            {
                var originWallet = await _walletRepository.GetByIdAsync(originWalletId);
                if (originWallet == null)
                {
                    _notificationHandler.AddNotification("WalletNotFound", "Falha ao obter carteira de origem com ID fornecido.");
                    return false;
                }

                var destinationWallet = await _walletRepository.GetByIdAsync(destinationWalletId);
                if (destinationWallet == null)
                {
                    _notificationHandler.AddNotification("WalletNotFound", "Falha ao obter carteira de destino com ID fornecido.");
                    return false;
                }

                switch (operation)
                {
                    case "Deposit":
                        originWallet.Balance += value;
                        originWallet.TransactionsId.Add(transactionId);
                        await _walletRepository.UpdateAsync(originWallet);
                        break;

                    case "Withdraw":
                        if (originWallet.Balance < value)
                        {
                            _notificationHandler.AddNotification("InsufficientFunds", "Saldo insuficiente para realizar o saque.");
                            return false;
                        }

                        originWallet.Balance -= value;
                        originWallet.TransactionsId.Add(transactionId);

                        await _walletRepository.UpdateAsync(originWallet);
                        break;
                    case "Transference":
                        if (originWallet.Balance < value)
                        {
                            _notificationHandler.AddNotification("InsufficientFunds", "Saldo insuficiente para realizar a transferencia.");
                            return false;
                        }

                        originWallet.Balance -= value;
                        destinationWallet.Balance += value;

                        originWallet.TransactionsId.Add(transactionId);
                        destinationWallet.TransactionsId.Add(transactionId);

                        await _walletRepository.UpdateAsync(originWallet);
                        await _walletRepository.UpdateAsync(destinationWallet);
                        break;

                    default:
                        _notificationHandler.AddNotification("InvalidOperation", "Operação inválida.");
                        return false;
                }
                transactionScope.Complete();
                return true;
            }
            catch (Exception ex)
            {
                _notificationHandler.AddNotification("UpdateFailed", $"Falha ao atualizar a carteira{ex.ToString()}");
                return false;
            }
        }
    }
}
