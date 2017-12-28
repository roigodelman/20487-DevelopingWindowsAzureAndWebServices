using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.ApplicationServer.Caching;

namespace MvcApplication1.Controllers
{
    public class LocationsController : ApiController
    {     
        public Location Get(int id)
        {
            Location location = null;

            // TODO: Place cache initialization here
            DataCacheFactory cacheFactory = new DataCacheFactory();
            DataCache cache = cacheFactory.GetDefaultCache();

            // TODO: Find the location entity in the cache
            string cacheKey = "location_" + id.ToString();
            location = cache.Get(cacheKey) as Location;

            if (location == null)
            {
                using (TravelCompanionContext context = new TravelCompanionContext())
                {
                    var locations = from l in context.Locations
                                    where l.LocationId == id
                                    select l;

                    location = locations.FirstOrDefault();

                    // TODO: Add the location to the cache
                    cache.Put(cacheKey, location);
                }
            }

            return location;
        }    
    }
}