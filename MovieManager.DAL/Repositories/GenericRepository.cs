using Microsoft.EntityFrameworkCore;
using MovieManager.DAL.Data;
using MovieManager.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace MovieManager.DAL.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly MovieDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(MovieDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = _context.Set<T>();
        }

        public async Task<T?> GetByIdAsync(
            int id,
            bool disableTracking = true,
            CancellationToken cancellationToken = default,
            params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;

            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (includes != null && includes.Length > 0)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }
            
            var keyName = _context.Model.FindEntityType(typeof(T))?
                .FindPrimaryKey()?.Properties.Select(x => x.Name).FirstOrDefault();

            if (string.IsNullOrEmpty(keyName))
            {
                throw new InvalidOperationException($"Nessuna chiave primaria trovata per l'entità {typeof(T).Name}");
            }

            return await query.FirstOrDefaultAsync(e => EF.Property<int>(e, keyName) == id, cancellationToken);
        }

        public async Task<IReadOnlyList<T>> GetAllAsync(
            Expression<Func<T, bool>>? predicate = null,
            bool disableTracking = true,
            CancellationToken cancellationToken = default,
            params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbSet;

            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (includes != null && includes.Length > 0)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            return await query.ToListAsync(cancellationToken);
        }
        
        public async Task<IReadOnlyList<T>> FindAsync(
            Expression<Func<T, bool>> predicate,
            CancellationToken cancellationToken = default)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate), "Il filtro di ricerca non può essere nullo.");
            }

            return await _dbSet.AsNoTracking().Where(predicate).ToListAsync(cancellationToken);
        }

        public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "Impossibile aggiungere un'entità nulla.");
            }

            await _dbSet.AddAsync(entity, cancellationToken);
        }

        public async Task UpdateAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "Impossibile aggiornare un'entità nulla.");
            }

            _dbSet.Update(entity);
        }

        public async Task RemoveAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "Impossibile rimuovere un'entità nulla.");
            }

            _dbSet.Remove(entity);
        }
        
        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }

        public void Delete<TEntity>(TEntity entity) where TEntity : class
        {
            throw new NotImplementedException();
        }

        public void Update(T entity)
        {
            throw new NotImplementedException();
        }
    }
}