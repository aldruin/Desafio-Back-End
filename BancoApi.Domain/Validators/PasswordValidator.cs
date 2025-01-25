using BancoApi.Domain.ValueObject;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BancoApi.Domain.Rules;
public class PasswordValidator : AbstractValidator<Password>
{
    private const string Pattern = @"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$";

    public PasswordValidator()
    {
        RuleFor(x => x.Value)
            .NotEmpty()
            .Must(BeValidPassword).WithMessage("A Senha deve ter no mínimo 8 caracteres, uma letra, um caracter especial e um número");
    }
    private bool BeValidPassword(string value) => Regex.IsMatch(value, Pattern);

}