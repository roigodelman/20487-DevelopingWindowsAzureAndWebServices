using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueYonder.DataAccess;
using BlueYonder.Entities;
using BlueYonder.Entities.Enums;

namespace BlueYonder.IntegrationTests
{
    // TODO: Mod 2: Exercise 1: Task 2b: Examine the content of the FlightScheduleDatabaseInitializer initializer class.
    // This class derives from the DropCreateDatabaseAlways so the database will be re-created every time we run the test
    // The Seed method contains all the initialization code
    class TravelCompanionDatabaseInitializer : DropCreateDatabaseAlways<TravelCompanionContext>
    {
        protected override void Seed(TravelCompanionContext context)
        {
            Traveler traveler1 = new Traveler
            {
                FirstName = "FirstName1",
                LastName = "LastName1",
                HomeAddress = "One microsoft road",
                MobilePhone = "555-555-5555",
                Passport = "AB123456789",
                TravelerUserIdentity = "aaabbbccc"
            };

            Traveler traveler2 = new Traveler
            {
                FirstName = "FirstName1",
                LastName = "LastName1",
                HomeAddress = "One microsoft road",
                MobilePhone = "555-555-5555",
                Passport = "AB987654321",
                TravelerUserIdentity = "dddeeefff"
            };

            context.Travelers.Add(traveler1);
            context.Travelers.Add(traveler2);
            context.SaveChanges();

            Location paris = new Location
            {
                City = "Paris",
                Country = "France",
                CountryCode = "FR",
                LocationTimeZone = TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time")
            };

            Location rome = new Location
            {
                City = "Rome",
                Country = "Italy",
                CountryCode = "IT",
                LocationTimeZone = TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time")
            };

            Location nyc = new Location
            {
                City = "New-York",
                Country = "United States",
                CountryCode = "US",
                State = "New-York",
                LocationTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time")
            };

            Flight flight1 = new Flight
            {
                FlightNumber = "BY001",
                Destination = paris,
                Source = rome,
                Schedules = new List<FlightSchedule> {
                    new FlightSchedule { 
                        Departure = new DateTime(2014, 2, 1, 8, 30, 0, DateTimeKind.Unspecified),
                        Duration = new TimeSpan(2, 30, 0)
                    }
                }
            };

            Flight flight2 = new Flight
            {
                FlightNumber = "BY002",
                Destination = nyc,
                Source = paris,
                Schedules = new List<FlightSchedule> {
                    new FlightSchedule { 
                        Departure = new DateTime(2014, 2, 3, 9, 20, 0, DateTimeKind.Unspecified),
                        Duration = new TimeSpan(7, 30, 0)
                    }
                }
            };

            Flight flight3 = new Flight
            {
                FlightNumber = "BY003",
                Destination = paris,
                Source = nyc,
                Schedules = new List<FlightSchedule> {
                    new FlightSchedule { 
                        Departure = new DateTime(2014, 4, 1, 9, 20, 0, DateTimeKind.Unspecified),
                        Duration = new TimeSpan(1, 30, 0)
                    }
                }
            };

            Flight flight4 = new Flight
            {
                FlightNumber = "BY004",
                Destination = rome,
                Source = paris
            };

            Reservation reservation1 = new Reservation
            {
                ReservationDate = DateTime.Now,
                DepartureFlight = new Trip
                {
                    FlightInfo = flight2.Schedules.First(),
                    Status = FlightStatus.Confirmed,
                    Class = SeatClass.Economy
                },
                ReturnFlight = new Trip
                {
                    FlightInfo = flight3.Schedules.First(),
                    Status = FlightStatus.Confirmed,
                    Class = SeatClass.Economy
                },
                ConfirmationCode = "1234",
                TravelerId = traveler1.TravelerId
            };

            Reservation reservation2 = new Reservation
            {
                ReservationDate = DateTime.Now,
                DepartureFlight = new Trip
                {
                    FlightInfo = flight3.Schedules.First(),
                    Status = FlightStatus.Confirmed,
                    Class = SeatClass.Business
                },
                ReturnFlight = new Trip
                {
                    FlightInfo = flight1.Schedules.First(),
                    Status = FlightStatus.Confirmed,
                    Class = SeatClass.Business
                },
                ConfirmationCode = "4321",
                TravelerId = traveler2.TravelerId
            };

            context.Flights.Add(flight1);
            context.Flights.Add(flight2);
            context.Flights.Add(flight3);
            context.Flights.Add(flight4);

            //TODO: Lab 02 Exercise 2, Task 6.1 : Add code to save the new reservations          
        }
    }
}