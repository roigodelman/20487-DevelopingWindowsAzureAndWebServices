using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BlueYonder.Entities
{
    public class FlightSchedule
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FlightScheduleId { get; set; }

        // Departure time is in time zone of departure location
        public DateTime Departure { get; set; }

        public DateTime? ActualDeparture { get; set; }
        
        public TimeSpan Duration { get; set; }

        public int FlightId { get; set; }

        [ForeignKey("FlightId")]
        public virtual Flight Flight { get; set; }
    }
}