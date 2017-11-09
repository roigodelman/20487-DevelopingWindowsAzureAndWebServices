using System;
using System.Collections.Generic;
using System.Linq;
using BlueYonder.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BlueYonder.Companion.Controllers.Tests
{
    [TestClass]
    public class LocationControllerTest
    {
        private List<Location> _locations;
        private LocationsController _controller;

        public LocationsController Controller { get => _controller; set => _controller = value; }

        [TestInitialize]
        public void Initialize()
        {
            var seattle = new Location
            {
                LocationId = 1,
                Country = "UnitedStates",
                City = "Seattle",
                CountryCode = "US",
                LocationTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time")
            };

            var paris = new Location
            {
                LocationId = 2,
                Country = "France",
                City = "Paris",
                CountryCode = "FR",
                LocationTimeZone = TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time")
            };

            _locations = new List<Location> { seattle, paris };

            var repository = new StubILocationRepository
            {
                GetAll = () => _locations.AsQueryable()
            };
            // Create an instance of controller with a fake repository that retrieve data from list instead of db
            _controller = new LocationsController();
        }

        [TestMethod]
        public void TestGetAllLocations()
        {
            // Get all locations from repository
            var collection = _controller.Get();
            // verify all locations are retrieved
            Assert.AreEqual(collection.Count(), _locations.Count);

        }
    }
}
