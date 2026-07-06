using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace MovieManager.DAL.Repositories.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(
            int id,
            bool disableTracking = true,
            CancellationToken cancellationToken = default,
            params Expression<Func<T, object>>[] includes);

        Task<IReadOnlyList<T>> GetAllAsync(
            Expression<Func<T, bool>>? predicate = null,
            bool disableTracking = true,
            CancellationToken cancellationToken = default,
            params Expression<Func<T, object>>[] includes);
        
        Task<IReadOnlyList<T>> FindAsync(
            Expression<Func<T, bool>> predicate,
            CancellationToken cancellationToken = default);

        Task AddAsync(T entity, CancellationToken cancellationToken = default);
        void Update(T entity);
        void Remove(T entity);
        
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}