using BlueYonder.Entities;

namespace BlueYonder.DataAccess.Interfaces
{
    public interface IReservationRepository : ISingleKeyEntityRepository<Reservation, int>
    {
    }
}
