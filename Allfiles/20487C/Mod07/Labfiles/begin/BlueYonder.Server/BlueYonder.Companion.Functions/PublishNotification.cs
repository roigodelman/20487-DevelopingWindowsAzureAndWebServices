using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.ServiceBus.Messaging;
using BlueYonder.Companion.Entities;
using BlueYonder.Entities;
using System;
using BlueYonder.Companion.Entities.Mappers;
using BlueYonder.Companion.WNS;

namespace BlueYonder.Companion.Functions
{
    public static class PublishNotification
    {
        [FunctionName("PublishNotification")]
        public static void Run([ServiceBusTrigger("FlightUpdatesQueue", AccessRights.Manage, Connection = "ServiceBusConnectionString")]BrokeredMessage message, TraceWriter log)
        {
            switch (message.ContentType)
            {
                case "Subscription":
                    HandleMessageContent(message.GetBody<RegisterNotificationsRequest>());
                    break;
                case "UpdatedSchedule":
                    HandleMessageContent(message.GetBody<FlightScheduleDTO>());
                    break;
                default:
                    throw new NotImplementedException($"Unable to handle unrecognized type: {message.GetType().Name}");
            }
        }

        private static void HandleMessageContent(FlightScheduleDTO updatedScheduleDto)
        {
            MessageHandler.Publish(updatedScheduleDto.ToFlightSchedule());
        }

        private static void HandleMessageContent(RegisterNotificationsRequest request)
        {
            MessageHandler.CreateSubscription(request);
        }
    }
}
