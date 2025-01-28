using BancoApi.Application.Notifications;
using BancoApi.Application.Users.Dtos;
using BancoApi.Application.Users.Services;
using BancoApi.Domain.Notifications;
using BancoApi.Domain.ValueObject;
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

    [HttpGet("id/{id}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id)
    {
        var user = await _userService.GetByIdAsync(id);
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

    [HttpGet("cpf/{cpf}")]
    public async Task<IActionResult> GetByCpfAsync([FromRoute] string cpf)
    {
        var user = await _userService.GetByCpfAsync(cpf);
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

    [HttpGet("email/{email}")]
    public async Task<IActionResult> GetByEmailAsync([FromRoute] string email)
    {
        var user = await _userService.GetByEmailAsync(email);
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

    [HttpPut("update")]
    public async Task<IActionResult> UpdateAsync([FromBody] UserDto dto)
    {
        var user = await _userService.UpdateAsync(dto);
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

    [HttpDelete("id/{id}")]
    public async Task<IActionResult> DeleteByIdAsync([FromRoute] Guid id)
    {
        var user = await _userService.RemoveByIdAsync(id);
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
