using WhiteHotel.Application.Common.Interfaces;
using WhiteHotel.Domain.Entities;
using WhiteHotel.Infrastructure.Data;

namespace WhiteHotel.Infrastructure.Repository
{
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {
        readonly ApplicationDbContext _db;

        public ApplicationUserRepository(ApplicationDbContext db) : base(db) 
        {
            _db = db;
        }
      
    }
}
