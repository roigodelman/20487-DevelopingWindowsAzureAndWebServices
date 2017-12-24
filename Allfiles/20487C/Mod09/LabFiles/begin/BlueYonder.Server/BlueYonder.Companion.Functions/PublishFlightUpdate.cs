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
    public static class PublishFlightUpdate
    {
        [FunctionName("PublishFlightUpdate")]
        public static void Run([ServiceBusTrigger("FlightUpdatesQueue", AccessRights.Manage, Connection = "ServiceBusConnectionString")]FlightScheduleDTO updatedScheduleDto, TraceWriter log)
        {
            MessageHandler.Publish(updatedScheduleDto.ToFlightSchedule());  
        }
    }
}
