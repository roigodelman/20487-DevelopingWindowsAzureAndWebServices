using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace HotelBooking
{
    [DataContract]
    public class Reservation
    {
        [DataMember]
        public string HotelName
        {
            get;
            set;
        }

        [DataMember]
        public string GuestName
        {
            get;
            set;
        }

        [DataMember]
        public DateTime CheckinDate
        {
            get;
            set;
        }

        [DataMember]
        public int NumberOfDays
        {
            get;
            set;
        }
    }
}
