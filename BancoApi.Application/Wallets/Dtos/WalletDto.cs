using BancoApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancoApi.Application.Wallets.Dtos;
public record WalletDto
{
    public Guid? Id { get; set; }
    public Guid? UserId { get; set; }
    public decimal? Balance { get; set; }
    public virtual List<Guid>? TransactionsId { get; set; }
}
