using BancoApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancoApi.Application.Wallets.Dtos;
public record WalletDto
{
    public Guid UserId { get; set; }
    public decimal? Balance { get; set; }
    public virtual ICollection<Transaction>? OriginTransactions { get; set; }
    public virtual ICollection<Transaction>? DestineTransactions { get; set; }
}
