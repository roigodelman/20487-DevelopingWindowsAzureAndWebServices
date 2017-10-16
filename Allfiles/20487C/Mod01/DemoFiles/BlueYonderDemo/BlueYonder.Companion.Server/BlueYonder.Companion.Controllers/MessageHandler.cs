using BlueYonder.Companion.Entities;
using BlueYonder.Companion.WNS.Notifications;
using BlueYonder.DataAccess.Repositories;
using BlueYonder.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueYonder.Companion.WNS.WorkerRole
{
    static class MessageHandler
    {
        static public void Publish(FlightSchedule updatedSchedule)
        {
            var flightsRepository = new FlightRepository();
            var reservationsRepository = new ReservationRepository();

            Flight updatedFlight = flightsRepository.GetFlight(updatedSchedule.FlightId);
            var reservations = reservationsRepository.FindBy(
                r => r.DepartureFlight.FlightScheduleID == updatedSchedule.FlightScheduleId);

            var travelers = from r in reservations
                            select r.TravelerId;

            var notification = new ToastNotificationTextAndImage
            {
                TargetClientDevices = travelers.ToList(),
                TextHeading = string.Format("Flight {0} Was Rescheduled", updatedFlight.FlightNumber),
                TextBodyWrap = string.Format("Flight {0} was rescheduled for {1}", updatedFlight.FlightNumber, updatedSchedule.Departure)
            };

            WNSManager.DispatchNotification(notification);
        }

        static public void CreateSubscription(RegisterNotificationsRequest request)
        {
            TravelerRepository travelersRepository = new TravelerRepository();
            Traveler traveler = travelersRepository.FindBy(t => t.TravelerUserIdentity == request.DeviceID).SingleOrDefault();

            if (traveler != null)
            {
                WNSManager.RegisterDevice(traveler.TravelerId, request.DeviceURI);
            }
        }
    }
}
