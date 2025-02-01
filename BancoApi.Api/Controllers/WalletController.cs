using BancoApi.Application.Notifications;
using BancoApi.Application.Wallets.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BancoApi.Api.Controllers;
[Route("api/[controller]")]
[Authorize]
[ApiController]
public class WalletController : ControllerBase
{
    private readonly IWalletService _walletService;
    private readonly INotificationHandler _notificationHandler;

    public WalletController(IWalletService walletService, INotificationHandler notificationHandler)
    {
        _walletService = walletService;
        _notificationHandler = notificationHandler;
    }

    [HttpGet("id")]
    public async Task<IActionResult> GetByUserIdAsync()
    {
        var loggedUser = HttpContext.User;
        var wallet = await _walletService.GetByUserIdAsync(loggedUser);
        var notifications = _notificationHandler.GetNotifications();
        if (wallet != null)
        {
            return Ok(new
            {
                Success = true,
                Data = wallet,
                Notifications = notifications
            });
        }
        return BadRequest(new
        {
            Success = false,
            Errors = notifications
        });
    }


    [HttpGet("cpf")]
    public async Task<IActionResult> GetByUserCpfAsync()
    {
        var loggedUser = HttpContext.User;
        var wallet = await _walletService.GetByUserCpfAsync(loggedUser);
        var notifications = _notificationHandler.GetNotifications();
        if (wallet != null)
        {
            return Ok(new
            {
                Success = true,
                Data = wallet,
                Notifications = notifications
            });
        }
        return BadRequest(new
        {
            Success = false,
            Errors = notifications
        });
    }

    [HttpGet("email")]
    public async Task<IActionResult> GetByUserEmailASync()
    {
        var loggedUser = HttpContext.User;
        var wallet = await _walletService.GetByUserEmailAsync(loggedUser);
        var notifications = _notificationHandler.GetNotifications();
        if (wallet != null)
        {
            return Ok(new
            {
                Success = true,
                Data = wallet,
                Notifications = notifications
            });
        }
        return BadRequest(new
        {
            Success = false,
            Errors = notifications
        });
    }
}
