using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueYonder.Entities.Enums;

namespace BlueYonder.Entities
{
    public class Trip
    {
        public int TripId { get; set; }

        public int FlightScheduleID { get; set; }

        //TODO : Lab 02, Exercise 1 Task 2.1 : Add Foreign Key attributes
        public FlightSchedule FlightInfo { get; set; }

        public FlightStatus Status { get; set; }

        public SeatClass Class { get; set; }
    }
}
