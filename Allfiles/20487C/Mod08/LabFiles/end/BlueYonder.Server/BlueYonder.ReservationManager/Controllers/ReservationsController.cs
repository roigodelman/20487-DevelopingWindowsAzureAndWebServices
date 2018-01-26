using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using System.Net.Http.Headers;
using BlueYonder.Companion.Entities;

namespace BlueYonder.ReservationManager.Controllers
{
    public class ReservationsController : Controller
    {
        string _companionService = WebConfigurationManager.AppSettings["webapi:BlueYonderCompanionService"];

        public ActionResult Index()
        {
            IEnumerable<TravelerDTO> travelers = null;
            using (var proxy = new HttpClient() { BaseAddress = new Uri(_companionService) })
            {                
                travelers = proxy.GetAsync("Travelers").Result.Content.
                    ReadAsAsync<IEnumerable<TravelerDTO>>().Result;
            }
            return View(travelers);
        }

        public PartialViewResult Reservations(int travelerId)
        {
            IEnumerable<ReservationDTO> reservations = null;
            using (var proxy = new HttpClient() { BaseAddress = new Uri(_companionService) })
            {                
                TravelerDTO traveler = proxy.GetAsync("travelers/" + travelerId).Result.Content.
                    ReadAsAsync<TravelerDTO>().Result;
                
                reservations = proxy.GetAsync("travelers/" + travelerId + "/reservations").Result.Content.
                    ReadAsAsync<IEnumerable<ReservationDTO>>().Result;

                ViewBag.UserId = traveler.TravelerUserIdentity;
            }
            
            return PartialView(reservations);
        }

        public ActionResult Approve(int id, string userId)
        {
            using (var proxy = new HttpClient() { BaseAddress = new Uri(_companionService) })
            {
                TripDTO trip = proxy.GetAsync("Trips/" + id).Result.Content.ReadAsAsync<TripDTO>().Result;
                if (trip.Status == FlightStatus.StandBy)
                {
                    trip.Status = FlightStatus.Confirmed;

                    proxy.PutAsJsonAsync("Trips/" + id, trip).Wait();

                    // TODO: Exercise 2: Task 3i: Notify the client about the update using service bus topics
                    NotifyTraveler(trip, userId, "ReservationConfirmed");
                }
            }
            return Redirect("/Reservations");
        }

        public ActionResult UpgradeClass(int id, string userId, SeatClass newClass)
        {
            using (var proxy = new HttpClient() { BaseAddress = new Uri(_companionService) })
            {
                TripDTO trip = proxy.GetAsync("Trips/" + id).Result.Content.ReadAsAsync<TripDTO>().Result;

                if ((trip.Class == SeatClass.Economy && newClass == SeatClass.Business) ||
                    (trip.Class == SeatClass.Business && newClass == SeatClass.First))
                {
                    trip.Class = newClass;

                    proxy.PutAsJsonAsync("Trips/" + id, trip).Wait();

                    // TODO: Exercise 2: Task 3i: Notify the client about the update using service bus topics
                    NotifyTraveler(trip, userId, "Upgrade");
                }
            }
            return Redirect("/Reservations");
        }

        private void NotifyTraveler(TripDTO trip, string travelerUserId, string updateType)
        {
            // TODO: Exercise 2: Task 3a: Connect to the service bus namespace

            // Create the topic if it does not exist already
            string connectionString =
                WebConfigurationManager.AppSettings["Microsoft.ServiceBus.ConnectionString"];

            string topicName =
                WebConfigurationManager.AppSettings["Microsoft.ServiceBus.TopicName"];

            var namespaceManager =
                NamespaceManager.CreateFromConnectionString(connectionString);

            // TODO: Exercise 2: Task 3b: Each client requires a different subscription so they 
            // will only get their own updates. Create a variable for the subscription name  
            // and set its value to the id of the client that created the order

            string travelerSubscriptionName = travelerUserId;

            // TODO: Exercise 2: Task 3c: Each subsription is for a different client,
            // create a filter that looks for a message sent for the specific client
            CorrelationFilter travelerFilter = new CorrelationFilter(travelerSubscriptionName);

            // TODO: Exercise 2: Task 3d: Verify the subscription exists, and create it if it doesn't
            if (!namespaceManager.SubscriptionExists(topicName, travelerSubscriptionName))
            {
                namespaceManager.CreateSubscription(topicName, travelerSubscriptionName, travelerFilter);
            }

            // TODO: Exercise 2: Task 3e: Connect to the topic
            TopicClient Client = TopicClient.CreateFromConnectionString(connectionString, topicName);

            // TODO: Exercise 2: Task 3f: Create the brokered message
            BrokeredMessage message = new BrokeredMessage(trip);

            // TODO: Exercise 2: Task 3g: Set the ClientId property so the message will be delivered
            // to the required subscription, and set the UpdateType property to reflect the 
            // reason this message was sent to the client

            // Set additional custom app-specific property
            message.CorrelationId = travelerSubscriptionName;
            message.Properties["UpdateType"] = updateType;

            // TODO: Exercise 2: Task 3h: Send the message to the topic
            Client.Send(message);
        }
    }
}
