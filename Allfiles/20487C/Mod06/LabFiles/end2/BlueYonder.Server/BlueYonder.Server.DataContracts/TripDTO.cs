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
    public class TripDTO
    {
        [DataMember]
        public int FlightScheduleID { get; set; }

        [DataMember]
        public FlightStatus Status { get; set; }

        [DataMember]
        public SeatClass Class { get; set; }
    }
}