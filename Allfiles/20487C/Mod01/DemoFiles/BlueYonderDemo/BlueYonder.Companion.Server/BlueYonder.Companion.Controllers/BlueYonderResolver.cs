using BlueYonder.DataAccess;
using BlueYonder.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Dependencies;

namespace BlueYonder.Companion.Controllers
{
    public class BlueYonderResolver : IDependencyResolver
    {
        public IDependencyScope BeginScope()
        {
            return this;
        }

        public object GetService(Type serviceType)
        {
            if (serviceType == typeof(FlightsController))
                return new FlightsController(new FlightRepository(), new LocationRepository());
            if (serviceType == typeof(LocationsController))
                return new LocationsController(new LocationRepository());
            if (serviceType == typeof(ReservationsController))
                return new ReservationsController(new ReservationRepository());
            if (serviceType == typeof (TravelersController))
            {
                var dbContext = new TravelCompanionContext();
                return new TravelersController(new TravelerRepository(dbContext), new ReservationRepository(dbContext), new FlightRepository(dbContext), new LocationRepository(dbContext));
            }
            if (serviceType == typeof(TripsController))
                return new TripsController(new ReservationRepository());
            if (serviceType == typeof(FrequentFlyersController))
                return new FrequentFlyersController();
            if (serviceType == typeof(FilesController))
                return new FilesController(new ReservationRepository());
            return null;
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return new List<object>();
        }

        public void Dispose()
        {
        }
    }
}
