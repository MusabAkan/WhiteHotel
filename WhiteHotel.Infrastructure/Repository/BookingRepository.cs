using WhiteHotel.Application.Common.Interfaces;
using WhiteHotel.Domain.Entities;
using WhiteHotel.Infrastructure.Data;

namespace WhiteHotel.Infrastructure.Repository
{
    public class BookingRepository : Repository<Booking>, IBookingRepository
    {
        private readonly ApplicationDbContext _db;

        public BookingRepository(ApplicationDbContext db) : base(db) 
        {
            _db = db;
        }
        
        public void Update(Booking entity)
        {
            _db.Bookings.Update(entity);
        }

       
    }
}
