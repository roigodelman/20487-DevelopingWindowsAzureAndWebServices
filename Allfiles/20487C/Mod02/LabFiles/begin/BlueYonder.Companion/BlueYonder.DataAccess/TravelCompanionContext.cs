using BlueYonder.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueYonder.DataAccess
{    
    public class TravelCompanionContext : DbContext
    {
        public TravelCompanionContext(string connectionName) : base(connectionName)
        {
        }

        public TravelCompanionContext() : this("TravelCompanion")
        {
        }

        public DbSet<Location> Locations { get; set; }
        public DbSet<Flight> Flights { get; set; }
        public DbSet<FlightSchedule> FlightSchedules { get; set; }
        public DbSet<Traveler> Travelers { get; set; }    

        //TODO : Lab 02, Exercise 1 Task 3.1 : Add Entities to the Context

    }
}
