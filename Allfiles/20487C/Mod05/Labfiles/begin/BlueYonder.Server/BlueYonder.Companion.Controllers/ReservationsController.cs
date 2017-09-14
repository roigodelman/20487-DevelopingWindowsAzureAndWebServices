using System;
using System.Net;
using System.Net.Http;
using System.ServiceModel;
using System.Web;
using System.Web.Http;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using BlueYonder.Companion.Entities;
using BlueYonder.Companion.Entities.Mappers;
using BlueYonder.DataAccess.Interfaces;
using BlueYonder.Entities;
using System.ServiceModel;

namespace BlueYonder.Companion.Controllers
{
    public class ReservationsController : ApiController
    {
        // TODO: Module 5: Exercise 3: Task 3.1: Create an instance of the channel factory 
        

        public IReservationRepository Reservations { get; set; }

        public ReservationsController(IReservationRepository reservations)
        {
            Reservations = reservations;
        }

        public HttpResponseMessage GetReservation(int id)
        {
            var reservation = Reservations.GetSingle(id);

            // Handling the HTTP status codes
            if (reservation != null)
                return Request.CreateResponse<ReservationDTO>
                    (HttpStatusCode.OK, reservation.ToReservationDTO());
            else
                return Request.CreateResponse(HttpStatusCode.NotFound);
        }

        public HttpResponseMessage GetReservations(int travelerId)
        {
            var reservations = Reservations.FindBy(r => r.TravelerId == travelerId);

            var reservationsDto =
                from r in reservations.ToList() select r.ToReservationDTO();

            return Request.CreateResponse(HttpStatusCode.OK, reservationsDto);
        }

        public HttpResponseMessage Post([FromBody]ReservationDTO reservation)
        {
            Reservation newReservation = reservation.FromReservationDTO();


            // TODO: Module 5: Exercise 3: Task 3.6: Call the booking service to create the reservation and get the confirmation code.

            Reservations.Add(newReservation);
            Reservations.Save();

            // creating the response, with three key features:
            // 1. the newly saved entity
            // 2. 201 Created status code
            // 3. Location header with the location of the new resource
            var response = Request.CreateResponse(HttpStatusCode.Created, newReservation);
            response.Headers.Location = new Uri(Request.RequestUri, newReservation.ReservationId.ToString());
            return response;
        }

        public HttpResponseMessage Delete(int id)
        {
            var reservation = Reservations.GetSingle(id);

            // returning 404 if the entity doesn't exist 
            if (reservation == null)
                return Request.CreateResponse(HttpStatusCode.NotFound);

            Reservations.Delete(reservation);
            Reservations.Save();
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        
        private string CreateReservationOnBackendSystem(Reservation reservation)
        {
            // TODO: Module 5: Exercise 3: Task 3.2: Uncomment the Dto creation objects.   
            /*TripDto departureFlight = new TripDto
            {
                FlightScheduleID = reservation.DepartureFlight.FlightScheduleID,
                Class = reservation.DepartureFlight.Class,
                Status = reservation.DepartureFlight.Status
            };

            TripDto returnFlight = null;
            if (reservation.ReturnFlight != null)
            {
                returnFlight = new TripDto
                {
                    FlightScheduleID = reservation.ReturnFlight.FlightScheduleID,
                    Class = reservation.ReturnFlight.Class,
                    Status = reservation.ReturnFlight.Status
                };
            }

            ReservationDto request = new ReservationDto
            {
                DepartureFlight = departureFlight,
                ReturnFlight = null,
                ReservationDate = reservation.ReservationDate,
                TravelerId = reservation.TravelerId
            };
             */

            // TODO: Module 5: Exercise 3: Task 3.2: Create a channel Factory 
            try
            {
                // TODO: Module 5: Exercise 3: Task 3.3: Call the service and return the result

            }

            // TODO: Module 5: Exercise 3: Task 3.4: Call the service and return the result
            catch (HttpException fault)
            {
                    
            }
            catch (Exception)
            {
                // TODO: Module 5: Exercise 3: Task 3.5: abort the communication in case of Exception
               
                throw;
            }
            return null;
        }

    }
}