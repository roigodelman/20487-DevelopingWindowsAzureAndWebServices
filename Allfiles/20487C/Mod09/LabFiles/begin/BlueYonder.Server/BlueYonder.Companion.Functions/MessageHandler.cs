using BlueYonder.Companion.Entities;
using BlueYonder.Companion.WNS;
using BlueYonder.Companion.WNS.Notifications;
using BlueYonder.DataAccess.Repositories;
using BlueYonder.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BlueYonder.Companion.Functions
{
    static class MessageHandler
    {
        static public void Publish(FlightSchedule updatedSchedule)
        {
            var connString = Environment.GetEnvironmentVariable("TravelCompanion", EnvironmentVariableTarget.Process);
            var flightsRepository = new FlightRepository(connString);
            var reservationsRepository = new ReservationRepository(connString);

            Flight updatedFlight = flightsRepository.GetFlight(updatedSchedule.Flight.FlightId);
            var reservations = reservationsRepository.FindBy(
                r => r.DepartureFlight.FlightScheduleID == updatedSchedule.FlightScheduleId);

            var travelers = from r in reservations
                            select r.TravelerId;

            var notification = new ToastNotificationTextAndImage
            {
                TargetClientDevices = travelers.ToList(),
                TextHeading = string.Format("Flight {0} Was Rescheduled", updatedFlight.FlightNumber),
                TextBodyWrap = string.Format("Flight {0} was rescheduled for {1}", updatedFlight.FlightNumber, updatedSchedule.ActualDeparture)
            };

            WNSManager.DispatchNotification(notification);
        }
    }
}
