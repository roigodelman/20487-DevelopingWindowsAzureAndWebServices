using BlueYonder.DataAccess;
using BlueYonder.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Transactions;
using BlueYonder.DataAccess.Repositories;

namespace BlueYonder.IntegrationTests
{
    
    // The database initializer create test data and clear any existing remains from previous runs, before running the tests
    [TestClass]
    public class FlightActions
    {
        [ClassInitialize]
        public static void TestInitialize(TestContext context)
        {
            (new TravelCompanionDatabaseInitializer()).InitializeDatabase(new TravelCompanionContext());
        }

        // Create the repository, perform the tested change, and then validate
        // the result in a separate repository to make sure there are no cached entities
        [TestMethod]
        public void InsertFlight()
        {
            FlightRepository flightRepository;
            LocationRepository locationRepository;
            Location from, to;

            string flightNumber = "BY999";
            DateTime takeOff = new DateTime(2014, 2, 1, 8, 30, 0); // Feb. 1st 2014, 8:30am

            using (locationRepository = new LocationRepository())
            {
                from = locationRepository.FindBy(l => l.City == "Paris").Single();
                to = locationRepository.FindBy(l => l.City == "New-York").Single();
            }

            using (flightRepository = new FlightRepository())
            {
                Flight newFlight = new Flight();
                newFlight.FlightNumber = flightNumber;
                newFlight.Destination = to;
                newFlight.Source = from;
                newFlight.Schedules = new List<FlightSchedule> {
                        new FlightSchedule
                        {
                            Departure  =  takeOff,
                            Duration = new TimeSpan(2, 30, 0)
                        }
                    };

                flightRepository.Add(newFlight);
                flightRepository.Save();
            }

            using (flightRepository = new FlightRepository())
            {
                Flight newFlight = flightRepository.GetFlight(flightNumber);
                Assert.AreEqual(newFlight.FlightNumber, flightNumber);
                Assert.AreEqual(newFlight.Destination.City, "New-York");
                Assert.AreEqual(newFlight.Source.City, "Paris");
                Assert.AreEqual(newFlight.Schedules.First().Departure, takeOff);
                Assert.AreEqual(newFlight.Schedules.First().Duration.TotalMinutes, 150);
            }
        }

        [TestMethod]
        public void DeleteFlight()
        {
            FlightRepository repository;
            using (repository = new FlightRepository())
            {
                Flight flight = repository.FindBy(f => f.FlightNumber == "BY004").Single();
                repository.Delete(flight);

                repository.Save();
            }

            using (repository = new FlightRepository())
            {
                Flight flight = repository.FindBy(f => f.FlightNumber == "BY004").FirstOrDefault();
                Assert.IsNull(flight);
            }
        }

        [TestMethod]
        public void UpdateFlight()
        {
            // This test checks if the repository is able of updating an entity 
            // that was updated outside of the repository (for example, 
            // an updated entity sent to the service)
             
            // The instance created here has the same values as the DB entity
            Flight flight = new Flight { FlightId = 3, FlightNumber = "BY002" };
            // Update the flight number
            flight.FlightNumber = "BY002_updated";

            //TODO: Lab 02 Exercise 2, Task 4.1 : Implement the UpdateFlight Method     
            FlightRepository repository;
            using (repository = new FlightRepository())
            {
                repository.Edit(flight);
                repository.Save();
            }

            using (repository = new FlightRepository())
            {
                Flight updatedFlight = repository.FindBy(f => f.FlightNumber == "BY002_updated").FirstOrDefault();
                Assert.IsNotNull(updatedFlight);
            }   
        }
     
        [TestMethod]
        public void UpdateUsingTwoRepositories()
        {
            LocationRepository locationRepository = new LocationRepository();
            FlightRepository flightRepository = new FlightRepository();
            Flight flight, flightFromDb;
            Location location;

            using (TransactionScope scope = new TransactionScope())
            {
                // Update flight and location
                flight = flightRepository.FindBy(f => f.FlightNumber == "BY001").Single();
                flight.FlightNumber = "BY001_updated";                
                // Since the flight was retrieved using the current repository, 
                // we don't need to call the Edit method
                flightRepository.Save();

                location = locationRepository.FindBy(l => l.City == "Rome").Single();
                location.City = "Rome_updated";
                // Since the location was retrieved using the current repository, 
                // we don't need to call the Edit method
                locationRepository.Save();

                //TODO: Lab 02, Exercise 2 Task 5.2 : Review the query for the updated flight that is inside the transaction scope               
                flightFromDb = (from f in flightRepository.GetAll()
                                where f.Source.City == "Rome_updated"
                                select f).FirstOrDefault();

                Assert.IsNotNull(flightFromDb);
                Assert.AreEqual(flightFromDb.FlightNumber, "BY001_updated");

                // Do not commit the transaction
                //scope.Complete();
            }


            //TODO: Lab 02, Exercise 2 Task 5.4 : Review the query for the updated flight that is outside the transaction scope
            flightFromDb = (from f in flightRepository.GetAll()
                            where f.Source.City == "Rome_updated"
                            select f).FirstOrDefault();

            Assert.IsNull(flightFromDb);

            locationRepository.Dispose();
            flightRepository.Dispose();
        }
    }
}