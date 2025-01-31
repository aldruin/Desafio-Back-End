using BancoApi.Domain.Entities;
using BancoApi.Domain.Repositories;
using BancoApi.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BancoApi.Infrastructure.Repositories;
public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(BancoApiDbContext context) : base(context)
    {
    }
    public async Task<User> GetByExpressionAsync(Expression<Func<User, bool>> expression)
    {
        return await this.Query
            .Include(u=>u.Wallet)
            .FirstOrDefaultAsync(expression);
    }
}
