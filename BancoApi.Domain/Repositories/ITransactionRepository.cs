using BancoApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BancoApi.Domain.Repositories;
public interface ITransactionRepository : IRepository<TransactionWallet>
{
    Task<TransactionWallet> GetByExpressionAsync(Expression<Func<TransactionWallet, bool>> expression);
}
