using BancoApi.Application.Transactions.Dtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancoApi.Application.Validators;
public class TransactionValidator : AbstractValidator<TransactionDto>
{
    public TransactionValidator()
    {
        RuleFor(t => t.OriginWalletId)
            .NotEmpty().WithMessage("A carteira de origem é obrigatória.")
            .Must(id => id != Guid.Empty).WithMessage("A carteira de origem é inválida.");

        RuleFor(t => t.DestinationWalletId)
            .NotEmpty().WithMessage("A carteira de destino é obrigatória.")
            .Must(id => id != Guid.Empty).WithMessage("A carteira de destino é inválida.");

        RuleFor(t => t.Value)
            .GreaterThan(0).WithMessage("O valor da transação deve ser maior que zero.");
    }
}
