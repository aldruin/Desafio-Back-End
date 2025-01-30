using BancoApi.Application.Notifications;
using BancoApi.Application.Wallets.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BancoApi.Api.Controllers;
[Route("api/[controller]")]
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

    [HttpGet("userid/{userId}")]
    public async Task<IActionResult> GetByUserIdAsync([FromRoute]Guid userId)
    {
        var wallet = await _walletService.GetByUserIdAsync(userId);
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


    [HttpGet("usercpf/{userCpf}")]
    public async Task<IActionResult> GetByUserCpfAsync([FromRoute] string userCpf)
    {
        var wallet = await _walletService.GetByUserCpfAsync(userCpf);
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

    [HttpGet("useremail/{userEmail}")]
    public async Task<IActionResult> GetByUserEmailASync([FromRoute] string userEmail)
    {
        var wallet = await _walletService.GetByUserEmailAsync(userEmail);
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
