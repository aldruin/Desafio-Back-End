﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BancoApi.Domain.Repositories;
public interface IRepository<T> where T : class
{
    Task<T> GetByIdAsync(Guid? id);
    Task<IEnumerable<T>> GetAllAsync();
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteByIdAsync(Guid id);
    Task<bool> AnyAsync(Expression<Func<T, bool>> expressao);
}