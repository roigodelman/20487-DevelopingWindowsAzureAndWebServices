using BlueYonder.Entities;

namespace BlueYonder.DataAccess.Interfaces
{
    public interface IReservationRepository : ISingleKeyEntityRepository<Reservation, int>
    {
        Trip GetTrip(int tripID);
        Reservation GetSingleNoTracking(int reservationID);
        void UpdateTrip(Trip originalEntity, Trip updatedEntity);
    }
}
