using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using BlueYonder.DataAccess.Interfaces;
using BlueYonder.Entities;
using BlueYonder.Companion.Controllers.DataTransferObjects;
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

        [Queryable]
        public IEnumerable<LocationDTO> Get()
        {
            var locations = Locations.GetAll().ToList();

            var locationsDTOs = from l in locations select l.ToLocationDTO();

            return locationsDTOs.ToList();
        }

        public LocationDTO Get(int id)
        {
            var location = Locations.GetSingle(id);

            return location.ToLocationDTO();
        }

        public WeatherForecast GetWeather (int locationId, DateTime date)
        {
            // Currently we won't be connecting to a real weather service 
            // so we'll create random weather forecasts
            return GeRandomWeather(date);
        }

        private WeatherForecast GeRandomWeather(DateTime date)
        {
            int maxTemperature;
            int minTemperature;

            // Set temperature bounds to create more logical results
            if (date.Month >= 10 && date.Month <= 3)
            {
                maxTemperature = 73;
                minTemperature = 28;
            }
            else 
            {
                maxTemperature = 96;
                minTemperature = 71;
            }

            // Get a random temperature
            Random rand = new Random();
            double temperature = rand.NextDouble() + 
                (double)rand.Next(minTemperature, maxTemperature);

            // Set possible weather conditions according to temperature
            WeatherCondition[] conditions;
            if (temperature >= 28 && temperature <= 75)
            {
                conditions = new WeatherCondition[] {
                    WeatherCondition.Snowy,WeatherCondition.Rainy, WeatherCondition.ThunderStorms, WeatherCondition.Cloudy};                    
            }
            else
            {
                conditions = new WeatherCondition[] {
                    WeatherCondition.Rainy,WeatherCondition.Cloudy, WeatherCondition.Sunny};                
            }

            // Return a random weather forecast
            return new WeatherForecast
            {
                TemperatureFahrenheit = Math.Round(temperature, 1),
                TemperatureCelcius = Math.Round((temperature - 32) * 5 / 9, 1),
                Condition = conditions[rand.Next(conditions.Length)]
            };
        }
    }
}