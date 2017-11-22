using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using BlueYonder.DataAccess.Interfaces;
using BlueYonder.Entities;
using BlueYonder.Companion.Controllers.DataTransferObjects;
using BlueYonder.Companion.Entities.Mappers;
using BlueYonder.Companion.Entities;
using BlueYonder.DataAccess.Repositories;

namespace BlueYonder.Companion.Controllers
{
    public class LocationsController : ApiController
    {
        public ILocationRepository Locations { get; set; }

        public LocationsController()
        {
            Locations = new LocationRepository();
        }

        public IEnumerable<Location> Get(string country = null,
                                 string state = null,
                                 string city = null)
        {
            var locations = Locations.GetAll();

            var result = from l in locations
                         where (country == null || l.Country.ToLower().Contains(country.ToLower())) &&
                               (state == null || l.State.ToLower().Contains(state.ToLower())) &&
                               (city == null || l.City.ToLower().Contains(city.ToLower()))
                         select l;

            return result.ToList();
        }


        public LocationDTO Get(int id)
        {
            var location = Locations.GetSingle(id);

            return location.ToLocationDTO();
        }

    }
}