using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace HotelBooking
{

    public interface IHotelBookingService
    {
        
        BookingResponse BookHotel(Reservation reservation);
    }
}
