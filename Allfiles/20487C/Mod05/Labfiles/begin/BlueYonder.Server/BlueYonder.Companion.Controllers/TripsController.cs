using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Hosting;
using System.Web.Http.Routing;
using System.Web;
using BlueYonder.Companion.Entities.Mappers;
using BlueYonder.Companion.Entities;
using BlueYonder.DataAccess.Interfaces;
using BlueYonder.Entities;

namespace BlueYonder.Companion.Controllers
{
    public class TripsController : ApiController
    {
        private const string IMAGE_URL_TEMPLATE = "{0}/Images/Thumbnails/{1}";

        public IReservationRepository Reservations { get; set; }

        public TripsController(IReservationRepository reservations)
        {
            Reservations = reservations;
        }

        public HttpResponseMessage Get(int id)
        {
            var departTrip = Reservations.GetAll().Where(r => r.DepartFlightScheduleID == id).Select(r => r.DepartureFlight);
            var returnTrip = Reservations.GetAll().Where(r => r.ReturnFlightScheduleID == id).Select(r => r.ReturnFlight);

            Trip trip = departTrip.Union(returnTrip).SingleOrDefault();

            if (!string.IsNullOrEmpty(trip.FlightInfo.Flight.Destination.ThumbnailImageFile))
            {
                trip.ThumbnailImage = ResolveImageUrl(trip.FlightInfo.Flight.Destination.ThumbnailImageFile);
            }
            
            if (trip == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, trip.ToTripDTO());
            }            
        }

        private string ResolveImageUrl(string relativeImagePath)
        {
            return string.Format(IMAGE_URL_TEMPLATE, Request.RequestUri.Authority, relativeImagePath);               
        }

        public HttpResponseMessage Put(int id, [FromBody]TripDTO trip)
        {
            Reservation reservation = Reservations.FindBy(r =>
                r.DepartFlightScheduleID == id ||
                r.ReturnFlightScheduleID == id).FirstOrDefault();

            if (reservation == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            Trip orignalEntity;           
            if (reservation.DepartFlightScheduleID == id)
            {
                orignalEntity = reservation.DepartureFlight;                
            }
            else
            {
                orignalEntity = reservation.ReturnFlight;                
            }                       

            Reservations.UpdateTrip(orignalEntity, trip.FromTripDTO());
            Reservations.Save();           

            return Request.CreateResponse(HttpStatusCode.OK);
        }       
    }
}
