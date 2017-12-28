using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MvcApplication1.Controllers
{
    public class LocationsController : ApiController
    {     
        public Location Get(int id)
        {
            Location location = null;

            // TODO: Place cache initialization here

            // TODO: Find the location entity in the cache

            if (location == null)
            {
                using (TravelCompanionContext context = new TravelCompanionContext())
                {
                    var locations = from l in context.Locations
                                    where l.LocationId == id
                                    select l;

                    location = locations.FirstOrDefault();

                    // TODO: Add the location to the cache
                }
            }

            return location;
        }    
    }
}