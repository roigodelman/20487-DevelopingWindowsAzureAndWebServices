using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlueYonder.Entities
{
    public class Reservation
    {
        public int ReservationId { get; set; }

        public int TravelerId { get; set; }

        public DateTime ReservationDate { get; set; }

        public string ConfirmationCode { get; set; }

        public int DepartFlightScheduleID { get; set; }

        //TODO : Lab 02, Exercise 1 Task 2.2 : Add Foreign Key attributes
        [ForeignKey("DepartFlightScheduleID")]
        public virtual Trip DepartureFlight { get; set; }

        //TODO : Lab 02, Exercise 1 Task 2.2 : mark property as nullable
        public int? ReturnFlightScheduleID { get; set; }

        //TODO : Lab 02, Exercise 1 Task 2.2 : Add Foreign Key attributes
        [ForeignKey("ReturnFlightScheduleID")]
        public virtual Trip ReturnFlight { get; set; }
    }
}
