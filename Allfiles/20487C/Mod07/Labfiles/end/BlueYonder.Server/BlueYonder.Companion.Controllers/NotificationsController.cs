using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using BlueYonder.Companion.Entities;
using Microsoft.ServiceBus.Messaging;

namespace BlueYonder.Companion.Controllers
{
    public class NotificationsController : ApiController
    {
        private static QueueClient Client;

        static NotificationsController()
        {
            Client = ServiceBusQueueHelper.ConnectToQueue();
        }


        [HttpPost]
        public HttpResponseMessage Register(RegisterNotificationsRequest request)
        {
            // TODO: Lab07, Exercise 2, Task 1.7 : Review the Register method
            var msg = new BrokeredMessage(request);
            msg.ContentType = "Subscription";
            Client.Send(msg);

            return Request.CreateResponse(HttpStatusCode.Created);
        }
    }
}
