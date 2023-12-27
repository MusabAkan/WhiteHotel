using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WhiteHotel.Application.Common.Interfaces;
using WhiteHotel.Application.Common.Utility;
using WhiteHotel.Domain.Entities;

namespace WhiteHotel.Infrastructure.Data
{
    public class DbInitializer : IDbInitializer
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _db;

        public DbInitializer(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext db)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _db = db;
        }

        public void Initialize()
        {
            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }

                if (!_roleManager.RoleExistsAsync(SD.Role_Admin).GetAwaiter().GetResult())
                {
                    _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin)).Wait();
                    _roleManager.CreateAsync(new IdentityRole(SD.Role_Customer)).Wait();
                    _userManager.CreateAsync(new ApplicationUser
                    {
                        UserName = "admin@musakan.com",
                        Email = "admin@musakan.com",
                        Name = "Bhrugen Patel",
                        NormalizedUserName = "ADMIN@MUSAKAN.COM",
                        NormalizedEmail = "ADMIN@MUSAKAN.COM",
                        PhoneNumber = "123456798",
                    }, "Admin123*").GetAwaiter().GetResult();

                    ApplicationUser user = _db.ApplicationUsers.FirstOrDefault(u => u.Email == "admin@musakan.com");
                    _userManager.AddToRoleAsync(user, SD.Role_Admin).GetAwaiter().GetResult();
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
