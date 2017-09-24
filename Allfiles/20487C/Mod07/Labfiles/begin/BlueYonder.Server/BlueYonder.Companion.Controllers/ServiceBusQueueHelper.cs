using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Microsoft.Azure;
using System.Configuration;

namespace BlueYonder.Companion.Controllers
{
    
    
    class ServiceBusQueueHelper
    {
        static readonly string QueueName = "FlightUpdatesQueue";

        
        public static QueueClient ConnectToQueue()
        {
            //TODO: Lab07, Exercise 2, Task 1.4 : Implement the ConnectToQueue Method.

            return null;

        }

    }
}
