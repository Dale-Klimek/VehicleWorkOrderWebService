namespace VehicleWorkOrder.Database
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<List<TEntity>> AllAsync();

        Task<List<TEntity>> AllIncludeAsync(params Expression<Func<TEntity, object>>[] includeProperties);

        Task<IEnumerable<TEntity>> FindByIncludeAsync(
            Expression<Func<TEntity, bool>> predicate,
            params Expression<Func<TEntity, object>>[] includeProperties);

        Task<IEnumerable<TEntity>> FindByAsync(
            Expression<Func<TEntity, bool>> predicate,
            int? skip = null,
            int? take = null);

        // ReSharper disable once TooManyArguments
        Task<IEnumerable<TEntity>> FindByAsync<T, TKey>(
            Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, TKey>> orderBy = null,
            int? skip = null,
            int? take = null);

        // ReSharper disable once TooManyArguments
        Task<IEnumerable<TEntity>> FindByDescAsync<T, TKey>(
            Expression<Func<TEntity, bool>> predicate,
            int? skip = null,
            int? take = null,
            Expression<Func<TEntity, TKey>> orderBy = null);

        Task InsertAsync(TEntity entity);

        Task InsertAsync(IEnumerable<TEntity> entities);

        ValueTask<TEntity> GetByIdAsync(object id);

        Task UpdateAsync(TEntity entity);

        Task UpdateAsync(IEnumerable<TEntity> entities);

        Task DeleteAsync(object id);

        Task DeleteAsync(IEnumerable<TEntity> entitiesToDelete);

        Task DeleteAsync(TEntity entityToDelete);
    }
}
