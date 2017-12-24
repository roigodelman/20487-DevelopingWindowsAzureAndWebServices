using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using BlueYonder.DataAccess.Interfaces;
using BlueYonder.Entities;
using BlueYonder.Companion.Entities.Mappers;
using BlueYonder.Companion.Entities;

namespace BlueYonder.Companion.Controllers
{
    public class LocationsController : ApiController
    {
        public ILocationRepository Locations { get; set; }

        public LocationsController(ILocationRepository locations)
        {
            Locations = locations;
        }

        public IEnumerable<LocationDTO> Get(
          string country = null,
          string state = null,
          string city = null)
        {
            var allLocations = Locations.GetAll();

            var locations =
                from l in allLocations
                where (country == null || l.Country.ToLower().Contains(country.ToLower())) &&
                    (state == null || l.State.ToLower().Contains(state.ToLower())) &&
                    (city == null || l.City.ToLower().Contains(city.ToLower()))
                select l;

            var dtos =
                locations
                    .ToArray()
                    .Select(l => l.ToLocationDTO())
                    .ToList();
            return dtos;
        }

        public LocationDTO Get(int id)
        {
            var location = Locations.GetSingle(id);

            return location.ToLocationDTO();
        }

        public DataTransferObjects.WeatherForecast GetWeather(int locationId, DateTime date)
        {            
            var service = new WeatherService();
            Location location = Locations.GetSingle(locationId);

            return service.GetWeather(location, date);
        }


    }
}