using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BlueYonder.Entities;
using BlueYonder.DataAccess.Interfaces;
using BlueYonder.Companion.Entities;
using FlightStatus = BlueYonder.Entities.FlightStatus;
using SeatClass = BlueYonder.Entities.SeatClass;

namespace BlueYonder.Companion.Controllers
{
    public class TravelersController : ApiController
    {
        private ITravelerRepository Travelers { get; set; }
        private IReservationRepository Reservations { get; set; }
        private IFlightRepository Flights { get; set; }
        private ILocationRepository Locations { get; set; }

        public TravelersController(ITravelerRepository travelers, IReservationRepository reservations, IFlightRepository flights, ILocationRepository locations)
        {
            Travelers = travelers;
            Reservations = reservations;
            Flights = flights;
            Locations = locations;
        }

        public IEnumerable<Traveler> Get()
        {
            var travelers = Travelers.GetAll();

            return travelers.ToList();
        }

        public HttpResponseMessage Get(string id)
        {
            var traveler = Travelers.FindBy(t => t.TravelerUserIdentity == id).FirstOrDefault();

            // Handling the HTTP status codes
            if (traveler != null)
                return Request.CreateResponse<Traveler>(HttpStatusCode.OK, traveler);
            else
                return Request.CreateResponse(HttpStatusCode.NotFound);
        }

        public HttpResponseMessage GetTravelerByUserIdentity([FromUri]string travelerUserIdentity)
        {
            var traveler = Travelers.FindBy(t=>t.TravelerUserIdentity == travelerUserIdentity).SingleOrDefault();

            // Handling the HTTP status codes
            if (traveler != null)
                return Request.CreateResponse<Traveler>(HttpStatusCode.OK, traveler);
            else
                return Request.CreateResponse(HttpStatusCode.NotFound);
        }

        //[HttpPost]
        //public HttpResponseMessage RegisterForNotifications
        //    ([FromBody]RegisterNotificationsRequest request)
        //{
        //    var traveler = Travelers.FindBy(t => t.TravelerUserIdentity == request.DeviceID).SingleOrDefault();

        //    // Handling the HTTP status codes
        //    if (traveler == null)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.NotFound);
        //    }
        //    else
        //    {
        //        WNSManager.RegisterDevice(request.DeviceID, request.DeviceURI);
        //        return Request.CreateResponse(HttpStatusCode.Created, request);
        //    }
        //}

        public HttpResponseMessage Post([FromBody]Traveler traveler)
        {
            // saving the new order to the database
            Travelers.Add(traveler);
            Travelers.Save();

            AddInitialData(traveler);

            // creating the response, with three key features:
            // 1. the newly saved entity
            // 2. 201 Created status code
            // 3. Location header with the location of the new resource
            var response = Request.CreateResponse(HttpStatusCode.Created, traveler);
            response.Headers.Location = new Uri(Request.RequestUri, traveler.TravelerId.ToString());
            return response;
        }

        public HttpResponseMessage Put(int id, [FromBody]Traveler traveler)
        {
            // returning 404 if the entity doesn't exist 
            if (Travelers.GetSingle(id) == null)
                return Request.CreateResponse(HttpStatusCode.NotFound);

            Travelers.Edit(traveler);
            Travelers.Save();
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        public HttpResponseMessage Delete(int id)
        {
            var reservation = Travelers.GetSingle(id);

            // returning 404 if the entity doesn't exist 
            if (reservation == null)
                return Request.CreateResponse(HttpStatusCode.NotFound);

            Travelers.Delete(reservation);
            Travelers.Save();
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        private void AddInitialData(Traveler traveler)
        {
            var seattle = Locations.FindBy(l => l.City == "Seattle").First();
            var paris = Locations.FindBy(l => l.City == "Paris").First();
            var rome = Locations.FindBy(l => l.City == "Rome").First();
            var newYork = Locations.FindBy(l => l.City == "New York").First();

            var now = DateTime.Now;

            Reservations.Add(CreateReservation(traveler, seattle, newYork, now, "1111"));
            Reservations.Add(CreateReservation(traveler, rome, newYork, now.AddDays(-36), "2222"));
            Reservations.Add(CreateReservation(traveler, paris, rome, now.AddDays(-29), "3333"));
            Reservations.Add(CreateReservation(traveler, newYork, rome, now.AddDays(-22), "4444"));
            Reservations.Add(CreateReservation(traveler, rome, seattle, now.AddDays(-15), "5555"));
            Reservations.Add(CreateReservation(traveler, seattle, paris, now.AddDays(-8), "6666"));
            Reservations.Add(CreateReservation(traveler, newYork, paris, now.AddDays(29), "7777"));

            Reservations.Save();
        }

        private Reservation CreateReservation(Traveler traveler, Location source, Location destination, DateTime start, string confirmationCode)
        {
            var departureSchedule =
                Flights
                    .GetFlightSchedules(source, destination, start, start.AddDays(7))
                    .First();

            var returnSchedule =
                Flights
                    .GetFlightSchedules(destination, source, departureSchedule.Departure.AddDays(1), departureSchedule.Departure.AddDays(8))
                    .First();

            var reservation = new Reservation
            {
                ReservationDate = start,
                DepartureFlight = new Trip
                {
                    FlightInfo = departureSchedule,
                    Status = FlightStatus.Confirmed,
                    Class = SeatClass.Economy
                },
                ReturnFlight = new Trip
                {
                    FlightInfo = returnSchedule,
                    Status = FlightStatus.Confirmed,
                    Class = SeatClass.Economy
                },
                ConfirmationCode = confirmationCode,
                TravelerId = traveler.TravelerId
            };
            return reservation;
        }
    }
}