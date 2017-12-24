using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BlueYonder.DataAccess.Interfaces;
using BlueYonder.Entities;
using System.ServiceModel;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using BlueYonder.Companion.Entities;
using BlueYonder.Companion.Entities.Mappers;
using BlueYonder.DataAccess.Repositories;

namespace BlueYonder.Companion.Controllers
{
    public class ReservationsController : ApiController
    {
        public IReservationRepository Reservations { get; set; }
        public ITravelerRepository Travelers { get; set; }

        public ReservationsController()
        {
            Reservations = new ReservationRepository();
            Travelers = new TravelerRepository();
        }

        public HttpResponseMessage Get(int id)
        {
            var reservation = Reservations.GetSingle(id);

            // Handling the HTTP status codes
            if (reservation != null)
                return Request.CreateResponse<ReservationDTO>
                    (HttpStatusCode.OK, reservation.ToReservationDTO());
            else
                return Request.CreateResponse(HttpStatusCode.NotFound);
        }

        public HttpResponseMessage Get(string travelerId)
        {
            var traveler = Travelers.FindBy(t => t.TravelerUserIdentity == travelerId).FirstOrDefault();
            if(traveler == null)
                return Request.CreateResponse(HttpStatusCode.NotFound);

            var reservations = Reservations.FindBy(r => r.TravelerId == traveler.TravelerId);

            var reservationsDto =
                from r in reservations.ToList() select r.ToReservationDTO();

            return Request.CreateResponse(HttpStatusCode.OK, reservationsDto);
        }

        public HttpResponseMessage Post([FromBody]ReservationDTO reservation)
        {
            // be availabe on the database before you persist the reservation. note that these schdules
            // might already be persisted if several consequent calls were made.
            // saving the new order to the database
            Reservation newReservation = reservation.FromReservationDTO();

            // newReservation.ConfirmationCode = confirmationCode;
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

    }
}