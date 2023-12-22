using WhiteHotel.Domain.Entities;

namespace WhiteHotel.Application.Common.Interfaces
{
    public interface IVillaRepository : IRepository<Villa>
    {
        void Update(Villa entity);
    }
}
