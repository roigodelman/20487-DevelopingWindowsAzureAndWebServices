using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace HotelBooking
{

    [ServiceContract]
    public interface IHotelBookingService
    {
        //the OperationContract attribute marks this method as an operation exposed by the service
        [OperationContract]
        BookingResponse BookHotel(Reservation reservation);
    }
}
