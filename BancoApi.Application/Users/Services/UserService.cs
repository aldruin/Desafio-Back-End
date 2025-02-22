﻿using BancoApi.Application.Notifications;
using BancoApi.Application.Users.Dtos;
using BancoApi.Application.Validators;
using BancoApi.Application.Wallets.Services;
using BancoApi.Domain.Entities;
using BancoApi.Domain.Repositories;
using BancoApi.Domain.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BancoApi.Application.Users.Services;
public class UserService : IUserService
{
    private readonly INotificationHandler _notificationHandler;
    private readonly IUserRepository _userRepository;
    private readonly IWalletService _walletService;

    public UserService(INotificationHandler notificationHandler, IUserRepository userRepository, IWalletService walletService)
    {
        _notificationHandler = notificationHandler;
        _userRepository = userRepository;
        _walletService = walletService;
    }
    
    public async Task<UserDto> CreateAsync(UserDto dto)
    {
        try
        {
            var validate = await new UserValidator().ValidateAsync(dto);
            if (!validate.IsValid)
            {
                foreach (var error in validate.Errors)
                {
                    _notificationHandler.AddNotification("InvalidUser", error.ErrorMessage);
                    return null;
                }
            }

            if (await _userRepository.AnyAsync(x=>x.Email.Value == dto.Email))
            {
                _notificationHandler.AddNotification("InvalidUser", "E-mail de Usuário já cadastrado.");
                return null;
            }

            if (await _userRepository.AnyAsync(x => x.Cpf == dto.Cpf))
            {
                _notificationHandler.AddNotification("InvalidUser", "CPF de Usuário já cadastrado.");
                return null;
            }

            var user = new User
            {
                Name = dto.Name,
                LastName = dto.LastName,
                Cpf = dto.Cpf,
                Email = new Email(dto.Email),
                Password = new Password(dto.Password)
            };

            user.SetPassword();

            await _userRepository.AddAsync(user);

            await _walletService.CreateAsync(user.Id);

            _notificationHandler.AddNotification("UserCreated", "Usuário criado com sucesso!");

            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Cpf = user.Cpf,
                LastName = user.LastName,
                Email = user.Email.Value
            };
        }
        catch(Exception ex)
        {
            _notificationHandler.AddNotification("UserCreateFailed", $"Erro ao criar o usuário: {ex.Message}");
            return null;
        }
    }

    public async Task<UserDto> GetByCpfAsync(ClaimsPrincipal loggedUser)
    {

        var cpf = loggedUser.FindFirst("userCpf")?.Value.ToString();

        if (cpf == null)
        {
            _notificationHandler.AddNotification("InvalidUserCPF", "CPF de usuário nulo ou em branco.");
            return null;
        }

        var user = await _userRepository.GetByExpressionAsync(u => u.Cpf == cpf);

        if (user == null)
        {
            _notificationHandler.AddNotification("UserNotFound", "Não foi encontrado o usuário com CPF fornecido.");
            return null;
        }


        _notificationHandler.AddNotification("UserFound", "Usuário com CPF fornecido obtido com sucesso.");
        return new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Cpf = user.Cpf,
            LastName = user.LastName,
            Email = user.Email.Value
        };
    }

    public async Task<UserDto> GetByEmailAsync(ClaimsPrincipal loggedUser)
    {
        var email = loggedUser.FindFirst(ClaimTypes.Email)?.Value.ToString();
        if (email == null)
        {
            _notificationHandler.AddNotification("InvalidUserCPF", "E-mail de usuário nulo ou em branco.");
            return null;
        }

        var user = await _userRepository.GetByExpressionAsync(u => u.Email.Value == email);

        if (user == null)
        {
            _notificationHandler.AddNotification("UserNotFound", "Não foi encontrado o usuário com E-mail fornecido.");
            return null;
        }
        _notificationHandler.AddNotification("UserFound", "Usuário com E-mail fornecido obtido com sucesso.");
        return new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Cpf = user.Cpf,
            LastName = user.LastName,
            Email = user.Email.Value
        };
    }

    public async Task<UserDto> GetByIdAsync(ClaimsPrincipal loggedUser)
    {
        var id = Guid.TryParse(loggedUser.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var userId) ? userId : Guid.Empty;


        if (id == Guid.Empty)
        {
            _notificationHandler.AddNotification("InvalidUserId", "O ID informado é inválido.");
            return null;
        }

        var user = await _userRepository.GetByIdAsync(id);

        if (user == null)
        {
            _notificationHandler.AddNotification("UserNotFound", "Não foi encontrado o usuário com ID fornecido");
            return null;
        }

        _notificationHandler.AddNotification("UserFound", "Usuário com ID fornecido obtido com sucesso.");

        return new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Cpf = user.Cpf,
            LastName = user.LastName,
            Email = user.Email.Value
        };

    }

    public async Task<UserDto> RemoveByIdAsync(ClaimsPrincipal loggedUser)
    {
        var id = Guid.TryParse(loggedUser.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var userId) ? userId : Guid.Empty;

        if (id == Guid.Empty)
        {
            _notificationHandler.AddNotification("InvalidUserId", "O ID informado é inválido.");
            return null;
        }

        var user = await _userRepository.GetByIdAsync(id);

        if (user == null)
        {
            _notificationHandler.AddNotification("UserNotFound", "Não foi possivel remover usuário com ID fornecido, usuário não encontrado.");
            return null;
        }

        await _userRepository.DeleteByIdAsync(id);

        _notificationHandler.AddNotification("UserDeleted", "Usuário deletado com sucesso.");

        return null;
    }

    public async Task<UserDto> UpdateAsync(ClaimsPrincipal loggedUser, UserDto dto)
    {
        if (dto.Id == Guid.Empty)
        {
            _notificationHandler.AddNotification("InvalidUserId", "O ID informado é inválido ou nulo.");
            return null;
        }

        var loggedUserId = Guid.TryParse(loggedUser.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var userId) ? userId : Guid.Empty;

        if (dto.Id != loggedUserId)
        {
            _notificationHandler.AddNotification("Unauthorized", "O ID do Usuario logado não coincide com o ID passado.");
            return null;
        }

        var user = await _userRepository.GetByIdAsync(dto.Id);

        if (user == null)
        {
            _notificationHandler.AddNotification("UserNotFound", "Não foi encontrado o usuário com ID fornecido");
            return null;
        }

        bool hasChanges = false;

        if (!string.IsNullOrWhiteSpace(dto.Name) && user.Name != dto.Name)
        {
            user.Name = dto.Name;
            hasChanges = true;
        }

        if (!string.IsNullOrWhiteSpace(dto.LastName) && user.LastName != dto.LastName)
        {
            user.LastName = dto.LastName;
            hasChanges = true;
        }

        if (!string.IsNullOrWhiteSpace(dto.Cpf) && user.Cpf != dto.Cpf)
        {
            user.Cpf = dto.Cpf;
            hasChanges = true;
        }

        if (!string.IsNullOrWhiteSpace(dto.Email) && user.Email.Value != dto.Email)
        {
            user.Email = new Email(dto.Email);
            hasChanges = true;
        }

        if (!string.IsNullOrWhiteSpace(dto.Password))
        {
            user.Password = new Password(dto.Password);
            user.SetPassword();
            hasChanges = true;
        }

        if (!hasChanges)
        {
            _notificationHandler.AddNotification("NothingToUpdate", "Não foi encontrado nenhum dado para atualizar.");
            return null;
        }


        await _userRepository.UpdateAsync(user);

        _notificationHandler.AddNotification("UsuarioAtualizado", "Os  dados de usuario foram atualizados com sucesso.");
        return new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            LastName = user.LastName,
            Cpf = user.Cpf,
            Email = user.Email.Value
        };
    }
}
