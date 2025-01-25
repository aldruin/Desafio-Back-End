using BancoApi.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancoApi.Domain.Entities;
public sealed class Transaction : Entity
{
    public Guid OriginWalletId { get; set; }
    public Wallet OriginWallet { get; set; }
    public Guid DestineWalletId { get; set; }
    public Wallet DestineWallet { get; set; }
    public decimal Value { get; set; }
    public DateTime TransactionDate { get; set; }

    public Transaction() { }
}
