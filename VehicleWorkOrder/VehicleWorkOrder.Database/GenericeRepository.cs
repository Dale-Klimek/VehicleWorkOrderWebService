namespace VehicleWorkOrder.Database
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;

    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly DbContext _context;

        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Reviewed. Suppression is OK here.")]
        private readonly DbSet<TEntity> _dbSet;

        public GenericRepository(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public virtual Task<List<TEntity>> AllAsync() => _dbSet.AsNoTracking().ToListAsync();

        public virtual Task<List<TEntity>> AllIncludeAsync(
            params Expression<Func<TEntity, object>>[] includeProperties) => GetAllIncluding(includeProperties).ToListAsync();

        public virtual async Task<IEnumerable<TEntity>> FindByIncludeAsync(
            Expression<Func<TEntity, bool>> predicate,
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var query = GetAllIncluding(includeProperties);
            // ReSharper disable once AsyncConverter.AsyncAwaitMayBeElidedHighlighting
            return await query.Where(predicate).ToListAsync().ConfigureAwait(false);
        }

        public virtual async Task<IEnumerable<TEntity>> FindByAsync(
            Expression<Func<TEntity, bool>> predicate,
            int? skip = null,
            int? take = null)
        {
            var query = _dbSet.AsNoTracking().Where(predicate);
            if (skip != null)
                query = query.Skip(skip.Value);
            if (take != null)
                query = query.Take(take.Value);
            // ReSharper disable once AsyncConverter.AsyncAwaitMayBeElidedHighlighting
            return await query.ToListAsync().ConfigureAwait(false);
        }

        // ReSharper disable once TooManyArguments
        public virtual async Task<IEnumerable<TEntity>> FindByAsync<T, TKey>(
            Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, TKey>> orderBy = null,
            int? skip = null,
            int? take = null)
        {
            var query = _dbSet.AsNoTracking().Where(predicate);
            if (orderBy != null)
                query = query.OrderBy(orderBy);
            if (skip != null)
                query = query.Skip(skip.Value);
            if (take != null)
                query = query.Take(take.Value);
            // ReSharper disable once AsyncConverter.AsyncAwaitMayBeElidedHighlighting
            return await query.ToListAsync().ConfigureAwait(false);
        }

        // ReSharper disable once TooManyArguments
        public virtual async Task<IEnumerable<TEntity>> FindByDescAsync<T, TKey>(
            Expression<Func<TEntity, bool>> predicate,
            int? skip = null,
            int? take = null,
            Expression<Func<TEntity, TKey>> orderBy = null)
        {
            var query = _dbSet.AsNoTracking().Where(predicate);
            if (orderBy != null)
                query = query.OrderByDescending(orderBy);
            if (skip != null)
                query = query.Skip(skip.Value);
            if (take != null)
                query = query.Take(take.Value);
            // ReSharper disable once AsyncConverter.AsyncAwaitMayBeElidedHighlighting
            return await query.ToListAsync().ConfigureAwait(false);
        }

        public virtual async Task InsertAsync(TEntity entity)
        {
            _dbSet.Add(entity);
            // ReSharper disable once AsyncConverter.AsyncAwaitMayBeElidedHighlighting
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public virtual async Task InsertAsync(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                _dbSet.Add(entity);
            }

            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        // careful this will introduce tracking object
        // return DbSet.Find(id);
        public virtual ValueTask<TEntity> GetByIdAsync(object id) => _dbSet.FindAsync(id);

        public virtual async Task UpdateAsync(TEntity entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            // ReSharper disable once AsyncConverter.AsyncAwaitMayBeElidedHighlighting
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public virtual async Task UpdateAsync(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                _dbSet.Attach(entity);
                _context.Entry(entity).State = EntityState.Modified;
            }
            // ReSharper disable once AsyncConverter.AsyncAwaitMayBeElidedHighlighting
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public virtual async Task DeleteAsync(object id) => await DeleteAsync(await GetByIdAsync(id).ConfigureAwait(false)).ConfigureAwait(false);

        public virtual async Task DeleteAsync(IEnumerable<TEntity> entitiesToDelete)
        {
            foreach (var entity in entitiesToDelete)
            {
                if (_context.Entry(entity).State == EntityState.Detached)
                {
                    _dbSet.Attach(entity);
                }
                _dbSet.Remove(entity);
            }

            // ReSharper disable once AsyncConverter.AsyncAwaitMayBeElidedHighlighting
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public virtual async Task DeleteAsync(TEntity entityToDelete)
        {
            if (_context.Entry(entityToDelete).State == EntityState.Detached)
            {
                _dbSet.Attach(entityToDelete);
            }
            _dbSet.Remove(entityToDelete);
            // ReSharper disable once AsyncConverter.AsyncAwaitMayBeElidedHighlighting
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        protected virtual IQueryable<TEntity> GetAllIncluding(
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> queryable = _dbSet.AsNoTracking();
            return includeProperties.Aggregate(
                queryable,
                (current, includeProperty) => current.Include(includeProperty));
        }
    }
}
