using Microsoft.EntityFrameworkCore;
using WhiteHotel.Domain.Entities;

namespace WhiteHotel.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            }

        public DbSet<Villa> Villas { get; set; }
    }
}
