using System.ServiceModel;
using BlueYonder.BookingService.Contracts;
using BlueYonder.BookingService.Contracts.Faults;
using BlueYonder.DataAccess.Interfaces;
using BlueYonder.DataAccess.Repositories;
using BlueYonder.Entities;

namespace BlueYonder.BookingService.Implementation
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class BookingService : IBookingService
    {
        public static readonly string ConnectionName = "BlueYonderServer";    

        public string CreateReservation(ReservationDto request)
        {
            // Verify the flight is valid
            if (request.DepartureFlight == null)
            {
                throw new FaultException<ReservationCreationFault>(
                    new ReservationCreationFault
                    {
                        Description = "Reservation must include a departure flight",
                        ReservationDate = request.ReservationDate
                    },
                    "Invalid flight info");
            }
            

            // Create reservation object with trips
            var reservation = new Reservation
            {
                TravelerId = request.TravelerId,
                ReservationDate = request.ReservationDate,
                DepartureFlight = new Trip
                {
                    Class = request.DepartureFlight.Class,
                    Status = request.DepartureFlight.Status,
                    FlightScheduleID = request.DepartureFlight.FlightScheduleID
                }
            };

            if (request.ReturnFlight != null)
            {
                reservation.ReturnFlight = new Trip
                {
                    Class = request.ReturnFlight.Class,
                    Status = request.ReturnFlight.Status,
                    FlightScheduleID = request.ReturnFlight.FlightScheduleID
                };
            }

            using (IReservationRepository reservationRepository = new ReservationRepository(ConnectionName))
            {
                reservation.ConfirmationCode = ReservationUtils.GenerateConfirmationCode(reservationRepository);
                reservationRepository.Add(reservation);
                reservationRepository.Save();
                return reservation.ConfirmationCode;
            }
        }
    }
}
