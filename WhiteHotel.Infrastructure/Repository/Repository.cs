using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using WhiteHotel.Application.Common.Interfaces;
using WhiteHotel.Infrastructure.Data;

namespace WhiteHotel.Infrastructure.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        readonly ApplicationDbContext _db;
        internal DbSet<T> _dbSet;
        public Repository(ApplicationDbContext context)
        {
            _db = context;
            _dbSet = _db.Set<T>();
        }
        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null)
        {
            IQueryable<T> queryable = _dbSet;

            if (filter != null)
                queryable = queryable.Where(filter);

            if (!string.IsNullOrWhiteSpace(includeProperties))
                foreach (var includeProp in includeProperties
                             .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    queryable = queryable.Include(includeProp);
                }

            return queryable.ToList();
        }
        public T Get(Expression<Func<T, bool>> filter, string? includeProperties = null)
        {
            IQueryable<T> queryable = _dbSet;

            if (filter != null)
                queryable = queryable.Where(filter);

            if (!string.IsNullOrWhiteSpace(includeProperties))
                foreach (var includeProp in includeProperties
                             .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    queryable = queryable.Include(includeProp);
                }

            return queryable.FirstOrDefault();
        }
        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }
        public void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }
        public bool Any(Expression<Func<T, bool>> filter)
        {
           return _dbSet.Any(filter);
        }
    }
}
