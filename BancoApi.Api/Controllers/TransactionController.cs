using BancoApi.Application.Notifications;
using BancoApi.Application.Transactions.Dtos;
using BancoApi.Application.Transactions.Services;
using BancoApi.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BancoApi.Api.Controllers;
[Route("api/[controller]")]
[Authorize]
[ApiController]
public class TransactionController : ControllerBase
{
    private readonly ITransactionService _transactionService;
    private readonly INotificationHandler _notificationHandler;

    public TransactionController(ITransactionService transactionService, INotificationHandler notificationHandler)
    {
        _transactionService = transactionService;
        _notificationHandler = notificationHandler;
    }

    [HttpPost("deposit")]
    public async Task<IActionResult> DepositAsync([FromBody] TransactionDto dto)
    {
        var loggedUser = HttpContext.User;
        var transaction = await _transactionService.DepositAsync(loggedUser, dto);
        var notifications = _notificationHandler.GetNotifications();

        if (transaction != null)
        {
            return Ok(new
            {
                Success = true,
                Data = transaction,
                Notifications = notifications
            });
        }
        return BadRequest(new
        {
            Success = false,
            Errors = notifications
        });
    }

    [HttpPost("withdraw")]
    public async Task<IActionResult> WithdrawAsync([FromBody] TransactionDto dto)
    {
        var loggedUser = HttpContext.User;
        var transaction = await _transactionService.WithdrawAsync(loggedUser, dto);
        var notifications = _notificationHandler.GetNotifications();

        if (transaction != null)
        {
            return Ok(new
            {
                Success = true,
                Data = transaction,
                Notifications = notifications
            });
        }
        return BadRequest(new
        {
            Success = false,
            Errors = notifications
        });
    }

    [HttpPost("transfer")]
    public async Task<IActionResult> TransferAsync([FromBody] TransactionDto dto)
    {
        var loggedUser = HttpContext.User;
        var transaction = await _transactionService.TransferAsync(loggedUser, dto);
        var notifications = _notificationHandler.GetNotifications();

        if (transaction != null)
        {
            return Ok(new
            {
                Success = true,
                Data = transaction,
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
