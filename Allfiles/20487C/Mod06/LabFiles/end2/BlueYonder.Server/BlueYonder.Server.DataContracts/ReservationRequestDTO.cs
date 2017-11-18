using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BlueYonder.Server.DataContracts
{
    [DataContract]
    public class ReservationRequestDTO
    {
        [DataMember]
        public int TravelerId { get; set; }

        [DataMember]
        public DateTime ReservationDate { get; set; }

        [DataMember]
        public TripDTO DepartureFlight { get; set; }

        [DataMember]
        public TripDTO ReturnFlight { get; set; }
    }
}
