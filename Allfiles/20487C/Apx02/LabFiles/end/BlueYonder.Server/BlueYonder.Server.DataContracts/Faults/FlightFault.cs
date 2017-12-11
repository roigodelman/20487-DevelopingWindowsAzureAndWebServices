using System.Runtime.Serialization;

namespace BlueYonder.Server.DataContracts.Faults
{
    [DataContract]
    public class FlightFault
    {
        [DataMember]
        public string FlightNumber { get; set; }

        public FlightFault(string flightNumber)
        {
            FlightNumber = flightNumber;
        }
    }
}
