using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace HotelBooking
{
    //this class implements the service contract 
    public class HotelBookingService : IHotelBookingService
    {
        //implementation of the service operation
        public BookingResponse BookHotel(Reservation reservation)
        {
            if (reservation.HotelName == "HotelA")
            {
                BookingResponse response = new BookingResponse() 
                    { IsApproved = true, BookingReference = "AR3254" };

                return response;
            }
            else
            {
                BookingResponse response = new BookingResponse() 
                { IsApproved = false };

                return response;
            }
        }
    }
}
