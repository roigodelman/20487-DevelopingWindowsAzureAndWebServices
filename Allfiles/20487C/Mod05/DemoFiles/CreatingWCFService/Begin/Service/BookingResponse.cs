using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelBooking
{

    public class BookingResponse
    {
        public bool IsApproved
        {
            get;
            set;
        }

        public string BookingReference
        {
            get;
            set;
        }
    }
}
