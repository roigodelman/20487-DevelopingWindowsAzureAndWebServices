using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace BlueYonder.Entities
{
    [DataContract]
    public class TravelerDTO
    {
        [DataMember]   
        public int TravelerId { get; set; }

        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public string LastName { get; set; }

        [DataMember]
        public string MobilePhone { get; set; }

        [DataMember]
        public string HomeAddress { get; set; }

        [DataMember]
        public string Passport { get; set; }    
    }
}