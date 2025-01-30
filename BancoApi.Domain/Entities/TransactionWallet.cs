using BancoApi.Domain.Base;
using BancoApi.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancoApi.Domain.Entities;
public sealed class TransactionWallet : Entity
{
    public Guid OriginWalletId { get; set; }
    public Wallet OriginWallet { get; set; }
    public Guid DestinationWalletId { get; set; }
    public Wallet DestinationWallet { get; set; }
    public decimal Value { get; set; }
    public DateTime TransactionDate { get; set; }
    public TransactionOperation Operation { get; set; }

    public TransactionWallet() { }
}
