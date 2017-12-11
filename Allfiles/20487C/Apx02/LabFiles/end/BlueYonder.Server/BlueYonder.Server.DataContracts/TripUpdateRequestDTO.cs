using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BlueYonder.Server.DataContracts
{
    [DataContract]
    public class TripUpdateRequestDTO
    {
        [DataMember]
        public string ReservationConfirmationCode { get; set; }

        [DataMember]
        public FlightDirections FlightDirection { get; set; }
        
        [DataMember]
        public TripDTO TripToUpdate { get; set; }
    }
}
