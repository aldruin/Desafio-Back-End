using BancoApi.Domain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancoApi.Domain.Rules;
public class UserValidator : AbstractValidator<User>
{
    public UserValidator()
    {   
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Email).SetValidator(new EmailValidator());
        RuleFor(x => x.Password).SetValidator(new PasswordValidator());
    }
}
