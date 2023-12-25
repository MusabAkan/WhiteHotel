using WhiteHotel.Domain.Entities;

namespace WhiteHotel.Application.Common.Interfaces
{
    public interface IAmenityRepository : IRepository<Amenity>
    {
        void Update(Amenity entity);
    }
}
