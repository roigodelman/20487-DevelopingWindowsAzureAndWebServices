using BlueYonder.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BlueYonder.Server.DataContracts
{
    [DataContract]
    public class FlightScheduleDTO
    {
        public FlightScheduleDTO(FlightSchedule sourceSchedule)
        {
            FlightScheduleId = sourceSchedule.FlightScheduleId;
            Departure = sourceSchedule.Departure;

            if (sourceSchedule.Flight != null)
            {
                FlightID = sourceSchedule.Flight.FlightId;
            }
        }

        public FlightScheduleDTO()
        {
        }

        [DataMember]       
        public int FlightScheduleId { get; set; }

        // Departure time is in time zone of departure location
        [DataMember]
        public DateTime Departure { get; set; }

        [DataMember]
        public int FlightID { get; set; }

        [DataMember]
        public LocationDTO Source { get;set;}

        [DataMember]
        public LocationDTO Destination { get; set; }           
    }
}
