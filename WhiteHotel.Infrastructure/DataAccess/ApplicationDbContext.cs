using Microsoft.EntityFrameworkCore;

namespace WhiteHotel.Infrastructure.DataAccess
{
    public class ApplicationDbContext :DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
                
        }
    }
}
