using BancoApi.Application.Notifications;
using BancoApi.Application.Validators;
using BancoApi.Application.Wallets.Dtos;
using BancoApi.Domain.Entities;
using BancoApi.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            UserId = wallet.UserId,
            Balance = wallet.Balance
        };

    }
}
