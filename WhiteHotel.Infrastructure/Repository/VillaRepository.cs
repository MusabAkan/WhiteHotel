using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using WhiteHotel.Application.Common.Interfaces;
using WhiteHotel.Domain.Entities;
using WhiteHotel.Infrastructure.Data;

namespace WhiteHotel.Infrastructure.Repository
{
    public class VillaRepository : IVillaRepository
    {
        readonly ApplicationDbContext _context;

        public VillaRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public IEnumerable<Villa> GetAll(Expression<Func<Villa, bool>>? filter = null, string? includeProperties = null)
        {
            IQueryable<Villa> queryable = _context.Set<Villa>();

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

        public Villa Get(Expression<Func<Villa, bool>> filter, string? includeProperties = null)
        {
            IQueryable<Villa> queryable = _context.Set<Villa>();

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

        public void Add(Villa entity)
        {
            _context.Add(entity);
        }

        public void Update(Villa entity)
        {
            _context.Update(entity);
        }

        public void Remove(Villa entity)
        {
            _context.Remove(entity);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
