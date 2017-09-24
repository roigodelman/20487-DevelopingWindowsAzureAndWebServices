using System;
using BlueYonder.DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using BlueYonder.DataAccess.Repositories;
using BlueYonder.Entities;
using System.Data.Entity;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core.Objects;
//using System.Data.Objects;

namespace BlueYonder.IntegrationTests
{
    [TestClass]
    public class FlightQueries
    {
        [ClassInitialize]
        public static void TestInitialize(TestContext context)
        {
            (new TravelCompanionDatabaseInitializer()).InitializeDatabase(new TravelCompanionContext());
        }

        [TestMethod]
        public void GetSingleFlight()
        {
            using (var repository = new FlightRepository())
            {
                var query = from f in repository.GetAll()
                            where f.FlightNumber == "BY001"
                            select f;

                var flight = query.FirstOrDefault();

                Assert.IsNotNull(flight);
            }
        }

        [TestMethod]
        public void GetFlightWithLocationsEagerLoad()
        {
            Flight flight;

            using (var repository = new FlightRepository())
            {
                var query = from f in repository.GetAll()
                            where f.FlightNumber == "BY001"
                            select f;

                query = query.Include(f => f.Source).Include(f => f.Destination);

                flight = query.FirstOrDefault();
            }

            Assert.IsNotNull(flight);
            Assert.IsNotNull(flight.Source);
            Assert.IsNotNull(flight.Destination);
        }

        [TestMethod]
        public void GetFlightWithLocationsLazyLoad()
        {
            Flight flight;

            using (var repository = new FlightRepository())
            {
                var query = from f in repository.GetAll()
                            where f.FlightNumber == "BY001"
                            select f;

                flight = query.FirstOrDefault();

                Assert.IsNotNull(flight);
                Assert.IsNotNull(flight.Source);
                Assert.IsNotNull(flight.Destination);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectDisposedException))]
        public void GetFlightWithLocationsFailedLazyLoad()
        {
            Flight flight;

            using (var repository = new FlightRepository())
            {
                var query = from f in repository.GetAll()
                            where f.FlightNumber == "BY001"
                            select f;

                flight = query.FirstOrDefault();
            }

            Assert.IsNotNull(flight);

            // We haven't eager loaded the flight source, 
            // and the context is disposed, so lazy load will fail
            Assert.IsNotNull(flight.Source);
        }

        [TestMethod]
        public void GetOrderedFilteredLocations()
        {
            using (TravelCompanionContext context = new TravelCompanionContext())
            {
                ObjectQuery<Location> query = ((IObjectContextAdapter)context).ObjectContext.CreateQuery<Location>(
                                              @"SELECT value l 
                                              FROM locations as l 
                                              WHERE l.state is null 
                                              ORDER BY l.city");

                List<Location> locations = query.ToList();

                Assert.AreEqual(locations.Count, 2);
                Assert.AreEqual(locations.ElementAt(0).City, "Paris");
                Assert.AreEqual(locations.ElementAt(1).City, "Rome");
            }
        }

        [TestMethod]
        public void GetFlightSchedulesByLocation()
        {
            using (TravelCompanionContext context = new TravelCompanionContext())
            {
                IEnumerable<FlightSchedule> flights = context.Database.SqlQuery<FlightSchedule>(
                                                       @"Select fs.*
                                                       from Flights fr
                                                       inner join Locations l
                                                       on fr.destination_LocationId = l.locationId 
                                                       inner join FlightSchedules fs
                                                       on fr.flightId = fs.flightId
                                                       where l.city = 'Paris'");

                Assert.AreEqual(flights.Count(), 2);
            }
        }
    }
}
