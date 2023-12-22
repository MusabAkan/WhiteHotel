using WhiteHotel.Domain.Entities;

namespace WhiteHotel.Application.Common.Interfaces
{
    public interface IVillaNumberRepository : IRepository<VillaNumber>
    {
        void Update(VillaNumber entity);
    }
}
