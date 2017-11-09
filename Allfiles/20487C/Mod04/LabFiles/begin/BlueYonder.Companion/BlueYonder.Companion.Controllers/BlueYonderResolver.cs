using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueYonder.DataAccess.Repositories;
using System.Web.Http.Dependencies;


namespace BlueYonder.Companion.Controllers
{
    public class BlueYonderResolver : IDependencyResolver
    {        
        public object GetService(Type serviceType)
        {
            if (serviceType == typeof(FlightsController))
                return new FlightsController(new FlightRepository(), new LocationRepository());
            if (serviceType == typeof(ReservationsController))
                return new ReservationsController(new ReservationRepository());
            if (serviceType == typeof(TravelersController))
                return new TravelersController(new TravelerRepository());
            if (serviceType == typeof(TripsController))
                return new TripsController(new ReservationRepository());

            // TODO: Lab 4: Exercise 1: Task 2.1: Add a resolver for the LocationsController class                       


            return null;
        }
        
        public IEnumerable<object> GetServices(Type serviceType)
        {
            return new List<object>();
        }
        
        public IDependencyScope BeginScope()
        {
            return this;
        }

        public void Dispose()
        {            
        }
    }
}
