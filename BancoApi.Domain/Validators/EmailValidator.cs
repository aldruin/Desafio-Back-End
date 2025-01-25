using BancoApi.Domain.ValueObject;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BancoApi.Domain.Rules;
public class EmailValidator : AbstractValidator<Email>
{
    private const string Pattern = @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";

    public EmailValidator()
    {
        RuleFor(x => x.Value)
            .NotEmpty()
            .Must(BeAEmailValid).WithMessage("Email inválido");
    }
    private bool BeAEmailValid(string value) => Regex.IsMatch(value, Pattern);
}
