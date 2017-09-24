using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Net.Http.Headers;
using BlueYonder.Companion.Entities;

namespace BlueYonder.FlightsManager.Controllers
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
                }
            }
            return Redirect("/Reservations");
        }
    }
}
