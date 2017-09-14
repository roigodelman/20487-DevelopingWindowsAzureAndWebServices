using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace HotelBooking
{
    [DataContract]
    public class BookingResponse
    {
        [DataMember]
        public bool IsApproved
        {
            get;
            set;
        }

        [DataMember]
        public string BookingReference
        {
            get;
            set;
        }
    }
}
