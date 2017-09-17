using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueYonder.Entities.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlueYonder.Entities
{
    public class Trip
    {
        public int TripId { get; set; }

        public int FlightScheduleID { get; set; }

        //TODO : Lab 02, Exercise 1 Task 2.1 : Add Foreign Key attributes
        [ForeignKey("FlightScheduleID")]
        public virtual FlightSchedule FlightInfo { get; set; }

        public FlightStatus Status { get; set; }

        public SeatClass Class { get; set; }
    }
}
