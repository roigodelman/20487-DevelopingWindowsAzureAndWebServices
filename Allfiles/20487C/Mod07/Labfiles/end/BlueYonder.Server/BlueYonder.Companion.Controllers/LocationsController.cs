using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using BlueYonder.DataAccess.Interfaces;
using BlueYonder.Entities;
using BlueYonder.Companion.Entities.Mappers;
using BlueYonder.Companion.Entities;
using System.Web.Http.OData;

namespace BlueYonder.Companion.Controllers
{
    public class LocationsController : ApiController
    {
        public ILocationRepository Locations { get; set; }

        public LocationsController(ILocationRepository locations)
        {
            Locations = locations;
        }

        [EnableQuery]
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
    }
}