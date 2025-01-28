using BancoApi.Domain.Entities;
using BancoApi.Domain.Repositories;
using BancoApi.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BancoApi.Infrastructure.Repositories;
public class WalletRepository : Repository<Wallet>, IWalletRepository
{
    public WalletRepository(BancoApiDbContext context) : base(context)
    {
    }
}
