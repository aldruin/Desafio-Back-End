using BancoApi.Application.Notifications;
using BancoApi.Application.Users.Dtos;
using BancoApi.Application.Users.Services;
using BancoApi.Domain.Notifications;
using BancoApi.Domain.ValueObject;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BancoApi.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly INotificationHandler _notificationHandler;

    public UserController(IUserService userService, INotificationHandler notificationHandler)
    {
        _userService = userService;
        _notificationHandler = notificationHandler;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateAsync([FromBody] UserDto dto)
    {
        var user = await _userService.CreateAsync(dto);
        var notifications = _notificationHandler.GetNotifications();
        if(user != null)
        {
            return Ok(new
            {
                Success = true,
                Data = user,
                Notifications = notifications
            }); 
        }
        return BadRequest(new
        {
            Success = false,
            Errors = notifications
        });
    }

    [Authorize]
    [HttpGet("id")]
    public async Task<IActionResult> GetByIdAsync()
    {
        var loggedUser = HttpContext.User;
        var user = await _userService.GetByIdAsync(loggedUser);
        var notifications = _notificationHandler.GetNotifications();
        if (user != null)
        {
            return Ok(new
            {
                Success = true,
                Data = user,
                Notifications = notifications
            });
        }
        return BadRequest(new
        {
            Success = false,
            Errors = notifications
        });
    }

    [Authorize]
    [HttpGet("cpf")]
    public async Task<IActionResult> GetByCpfAsync()
    {
        var loggedUser = HttpContext.User;
        var user = await _userService.GetByCpfAsync(loggedUser);
        var notifications = _notificationHandler.GetNotifications();
        if (user != null)
        {
            return Ok(new
            {
                Success = true,
                Data = user,
                Notifications = notifications
            });
        }
        return BadRequest(new
        {
            Success = false,
            Errors = notifications
        });
    }

    [Authorize]
    [HttpGet("email")]
    public async Task<IActionResult> GetByEmailAsync()
    {
        var loggedUser = HttpContext.User;
        var user = await _userService.GetByEmailAsync(loggedUser);
        var notifications = _notificationHandler.GetNotifications();
        if (user != null)
        {
            return Ok(new
            {
                Success = true,
                Data = user,
                Notifications = notifications
            });
        }
        return BadRequest(new
        {
            Success = false,
            Errors = notifications
        });
    }

    [Authorize]
    [HttpPut("update")]
    public async Task<IActionResult> UpdateAsync([FromBody] UserDto dto)
    {
        var loggedUser = HttpContext.User;
        var user = await _userService.UpdateAsync(loggedUser, dto);
        var notifications = _notificationHandler.GetNotifications();
        if (user != null)
        {
            return Ok(new
            {
                Success = true,
                Data = user,
                Notifications = notifications
            });
        }
        return BadRequest(new
        {
            Success = false,
            Errors = notifications
        });
    }

    [Authorize]
    [HttpDelete("id")]
    public async Task<IActionResult> DeleteByIdAsync()
    {
        var loggedUser = HttpContext.User;
        var user = await _userService.RemoveByIdAsync(loggedUser);
        var notifications = _notificationHandler.GetNotifications();
        if (user != null)
        {
            return Ok(new
            {
                Success = true,
                Data = user,
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
