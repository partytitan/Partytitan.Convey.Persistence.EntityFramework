using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Convey.Types;
using Microsoft.EntityFrameworkCore;
using Partytitan.Convey.Persistence.EntityFramework.Repositories.Interfaces;

namespace Partytitan.Convey.Persistence.EntityFramework.Repositories
{
    public class EfRepository<TDbContext, TEntity, TIdentifiable> : IEfRepository<TDbContext, TEntity, TIdentifiable>
        where TDbContext : DbContext where TEntity : class, IIdentifiable<TIdentifiable>
    {
        private readonly TDbContext _database;

        public EfRepository(TDbContext database)
        {
            _database = database;
            Collection = database.Set<TEntity>();
        }

        public DbSet<TEntity> Collection { get; }

        public Task<TEntity> GetAsync(TIdentifiable id)
            => GetAsync(e => e.Id.Equals(id));

        public Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate)
            => Collection.AsQueryable().SingleOrDefaultAsync(predicate);

        public async Task<IReadOnlyList<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
            => await Collection.Where(predicate).ToListAsync();

        public async Task AddAsync(TEntity entity)
        {
            await Collection.AddAsync(entity);
            await _database.SaveChangesAsync();
        }

        public async Task UpdateAsync(TEntity entity)
        { 
            Collection.Update(entity);
            await _database.SaveChangesAsync();
        }

        public async Task DeleteAsync(TIdentifiable id)
        {
            var entity = await Collection.FirstOrDefaultAsync(e => e.Id.Equals(id));
            await DeleteAsync(entity);
        }

        public async Task DeleteAsync(TEntity entity)
        {
            Collection.Remove(entity);
            await _database.SaveChangesAsync();
        }

        public Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)
            => Collection.AnyAsync(predicate);
    }
}
