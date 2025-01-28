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

        RuleFor(t => t.DestineWalletId)
            .NotEmpty().WithMessage("A carteira de destino é obrigatória.")
            .Must(id => id != Guid.Empty).WithMessage("A carteira de destino é inválida.")
            .NotEqual(t => t.OriginWalletId).WithMessage("A carteira de origem e destino devem ser diferentes.");

        RuleFor(t => t.Value)
            .GreaterThan(0).WithMessage("O valor da transação deve ser maior que zero.");
    }
}
