using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using BlueYonder.Model;

namespace BlueYonder.MVC.Controllers
{
    public class LocationsController : ApiController
    {
        private BlueYonderEntities db = new BlueYonderEntities();

        // GET api/Locations
        public IEnumerable<Location> GetLocations()
        {
            return db.Locations.ToList().ConvertAll(location => new Location()
                                                                    {
                                                                        City = location.City,
                                                                        Country = location.Country,
                                                                        CountryCode = location.CountryCode,
                                                                        LocationId = location.LocationId,
                                                                        State = location.State,
                                                                        ThumbnailImageFile = location.ThumbnailImageFile,
                                                                        TimeZoneId = location.TimeZoneId,
                                                                    });
        }

        // GET api/Locations/5
        public Location GetLocation(int id)
        {
            Location location = db.Locations.Find(id);
            if (location == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return location;
        }

        // PUT api/Locations/5
        public HttpResponseMessage PutLocation(int id, Location location)
        {
            if (ModelState.IsValid && id == location.LocationId)
            {
                db.Entry(location).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // POST api/Locations
        public HttpResponseMessage PostLocation(Location location)
        {
            if (ModelState.IsValid)
            {
                db.Locations.Add(location);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, location);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = location.LocationId }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/Locations/5
        public HttpResponseMessage DeleteLocation(int id)
        {
            Location location = db.Locations.Find(id);
            if (location == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.Locations.Remove(location);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, location);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}