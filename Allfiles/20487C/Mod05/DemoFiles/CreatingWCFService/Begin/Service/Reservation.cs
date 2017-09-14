using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace HotelBooking
{

    public class Reservation
    {

        public string HotelName
        {
            get;
            set;
        }

        public string GuestName
        {
            get;
            set;
        }
       
        public DateTime CheckinDate
        {
            get;
            set;
        }
       
        public int NumberOfDays
        {
            get;
            set;
        }
    }
}
