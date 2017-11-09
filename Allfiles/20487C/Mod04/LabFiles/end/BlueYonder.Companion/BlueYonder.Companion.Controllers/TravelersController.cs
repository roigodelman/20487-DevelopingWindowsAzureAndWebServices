using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BlueYonder.Entities;
using BlueYonder.DataAccess.Interfaces;
using BlueYonder.DataAccess.Repositories;
using BlueYonder.Companion.Controllers.ActionFilters;

namespace BlueYonder.Companion.Controllers
{
    public class TravelersController : ApiController
    {
        private ITravelerRepository Travelers { get; set; }

        public TravelersController(ITravelerRepository travelers)
        {
            Travelers = travelers;
        }

        public IEnumerable<Traveler> Get()
        {
            var travelers = Travelers.GetAll();

            return travelers.ToList();
        }



        public HttpResponseMessage Get(string id)
        {
            var traveler = Travelers.FindBy(t=>t.TravelerUserIdentity == id).FirstOrDefault();

            // Handling the HTTP status codes
            if (traveler != null)
                return Request.CreateResponse<Traveler>(HttpStatusCode.OK, traveler);
            else
                return Request.CreateResponse(HttpStatusCode.NotFound);
        }

        [ModelValidation]
        public HttpResponseMessage Post(Traveler traveler)
        {
            // saving the new order to the database
            Travelers.Add(traveler);
            Travelers.Save();

            // creating the response, with three key features:
            // 1. the newly saved entity
            // 2. 201 Created status code
            // 3. Location header with the location of the new resource
            var response = Request.CreateResponse(HttpStatusCode.Created, traveler);
            response.Headers.Location = new Uri(Request.RequestUri, traveler.TravelerId.ToString());
            return response;
        }

        [ModelValidation]
        public HttpResponseMessage Put(int id, Traveler traveler)
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
            var traveler = Travelers.GetSingle(id);

            // returning 404 if the entity doesn't exist 
            if (traveler == null)
                return Request.CreateResponse(HttpStatusCode.NotFound);

            Travelers.Delete(traveler);
            Travelers.Save();
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}