using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlueYonder.Companion.Controllers.DataTransferObjects
{
    public class ScheduleDto
    {
        public DateTime Departure { get; set; }
        public DateTime Arrival { get; set; }
        public TimeSpan Duration { get; set; }
        public int FlightScheduleId { get; set; }
    }
}
