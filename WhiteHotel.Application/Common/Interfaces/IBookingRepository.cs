using WhiteHotel.Domain.Entities;

namespace WhiteHotel.Application.Common.Interfaces
{
    public interface IBookingRepository : IRepository<Booking>
    {
        void Update(Booking entity);
      
    }
}
