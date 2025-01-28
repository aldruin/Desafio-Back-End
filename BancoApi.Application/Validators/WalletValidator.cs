using BancoApi.Application.Wallets.Dtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancoApi.Application.Validators;
public class WalletValidator : AbstractValidator<WalletDto>
{
    public WalletValidator()
    {
        RuleFor(w => w.UserId)
            .NotEmpty().WithMessage("O ID do usuário é obrigatório.")
            .Must(id => id != Guid.Empty).WithMessage("O ID do usuário é inválido.");
    }
}