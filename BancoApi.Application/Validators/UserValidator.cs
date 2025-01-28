using BancoApi.Application.Users.Dtos;
using BancoApi.Domain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancoApi.Application.Validators;
public class UserValidator : AbstractValidator<UserDto>
{
    public UserValidator()
    {
        RuleFor(u => u.Name)
            .NotEmpty().WithMessage("O primeiro nome é obrigatório");

        RuleFor(u => u.LastName)
            .NotEmpty().WithMessage("O ultimo nome é obrigatório");

        RuleFor(u => u.Cpf)
            .NotEmpty().WithMessage("O CPF é obrigatório")
            .Matches(@"^\d{3}\.\d{3}\.\d{3}-\d{2}$|^\d{11}$")
            .WithMessage("CPF inválido. O formato correto é 000.000.000-00 ou 00000000000.");

        RuleFor(u => u.Email)
            .NotEmpty().WithMessage("O endereço de E-mail é obrigatório")
            .Matches(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*").WithMessage("O endereço de E-mail é obrigatório");

        RuleFor(u => u.Password)
            .NotEmpty().WithMessage("A senha é obrigatória")
            .Matches(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$")
            .WithMessage("A senha deve ter no mínimo 8 caracteres, uma letra, um caractere especial e um número");
    }
}
