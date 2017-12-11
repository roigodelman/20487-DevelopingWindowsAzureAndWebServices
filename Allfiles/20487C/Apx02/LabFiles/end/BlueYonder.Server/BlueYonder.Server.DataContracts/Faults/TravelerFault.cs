using System.Runtime.Serialization;

namespace BlueYonder.Server.DataContracts.Faults
{
    [DataContract]
    public class TravelerFault
    {
        [DataMember]
        public int TravelerId { get; set; }

        public TravelerFault(int travelerId)
        {
            TravelerId = travelerId;
        }
    }
}
