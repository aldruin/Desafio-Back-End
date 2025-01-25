using BancoApi.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancoApi.Domain.Entities;
public class Wallet : Entity
{
    public Guid UserId { get; set; }
    public User User { get; set; }
    public decimal Balance { get; set; }
    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

    public Wallet() { }
}
