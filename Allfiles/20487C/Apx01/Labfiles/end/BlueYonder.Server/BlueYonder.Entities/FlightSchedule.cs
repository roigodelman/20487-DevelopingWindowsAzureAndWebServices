using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace BlueYonder.Entities
{
    [DataContract]
    public class FlightSchedule
    {
        [DataMember]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FlightScheduleId { get; set; }

        // Departure time is in time zone of departure location
        [DataMember]
        public DateTime Departure { get; set; }

        [DataMember]
        public DateTime? ActualDeparture { get; set; }
        
        [DataMember]
        public TimeSpan Duration { get; set; }

//        [DataMember]
//        public int FlightId { get; set; }

        [JsonIgnore]
        [XmlIgnore]
        public virtual Flight Flight { get; set; }
    }
}
