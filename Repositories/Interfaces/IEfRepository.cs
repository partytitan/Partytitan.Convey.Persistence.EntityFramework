using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Convey.Types;
using Microsoft.EntityFrameworkCore;

namespace Partytitan.Convey.Persistence.EntityFramework.Repositories.Interfaces
{
    public interface IEfRepository<TDbContext, TEntity, in TIdentifiable> where TDbContext : DbContext
        where TEntity : class, IIdentifiable<TIdentifiable>
    {
        DbSet<TEntity> Collection { get; }
        Task<TEntity> GetAsync(TIdentifiable id);
        Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate);
        Task<IReadOnlyList<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);
        Task AddAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(TIdentifiable id);
        Task DeleteAsync(TEntity entity);
        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate);
    }
}
