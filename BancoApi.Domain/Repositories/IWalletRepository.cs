﻿using BancoApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BancoApi.Domain.Repositories;
public interface IWalletRepository : IRepository<Wallet>
{
    Task<Wallet> GetByExpressionAsync(Expression<Func<Wallet, bool>> expression);
}
