using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BlueYonder.Entities;
using BlueYonder.DataAccess.Interfaces;
using BlueYonder.DataAccess.Repositories;

namespace BlueYonder.Companion.Controllers
{
    public class TravelersController : ApiController
    {
        private ITravelerRepository Travelers { get; set; }

        public TravelersController()
        {
            Travelers = new TravelerRepository();
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

        public HttpResponseMessage Post(Traveler traveler)
        {
            // saving the new order to the database
            Travelers.Add(traveler);
            Travelers.Save();

            // creating the response, the newly saved entity and 201 Created status code
            var response = Request.CreateResponse(HttpStatusCode.Created, traveler);

            response.Headers.Location = new Uri(Request.RequestUri, traveler.TravelerId.ToString());
            return response;
        }

        public HttpResponseMessage Put(string id, Traveler traveler)
        {
            // returning 404 if the entity doesn't exist 
            if (Travelers.FindBy(t => t.TravelerUserIdentity == id).FirstOrDefault() == null)
                return Request.CreateResponse(HttpStatusCode.NotFound);

            Travelers.Edit(traveler);
            Travelers.Save();
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        public HttpResponseMessage Delete(string id)
        {
            var traveler = Travelers.FindBy(t => t.TravelerUserIdentity == id).FirstOrDefault();

            // returning 404 if the entity doesn't exist 
            if (traveler == null)
                return Request.CreateResponse(HttpStatusCode.NotFound);

            Travelers.Delete(traveler);
            Travelers.Save();
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}