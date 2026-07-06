using MovieManager.DAL.Data;
using MovieManager.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MovieManager.DAL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MovieDbContext _context;
        private readonly Dictionary<Type, object> _repositories;

        public UnitOfWork(MovieDbContext context)
        {
            _context = context;
        }

        public IGenericRepository<T> Repository<T>() where T : class 
        {
            var type = typeof(T);

            if(_repositories.TryGetValue(type, out var repository))
            {
                return (IGenericRepository<T>)repository;
            }

            var newRepository = new GenericRepository<T>(_context);
            _repositories[type] = newRepository;

            return newRepository;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
}
}
