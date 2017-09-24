using BlueYonder.Companion.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Mvc;

namespace BlueYonder.FlightsManager.Controllers
{
    public class FlightsController : Controller
    {
        string _companionService = WebConfigurationManager.AppSettings["webapi:BlueYonderCompanionService"];

        public ActionResult Index()
        {
            IEnumerable<LocationDTO> locations = null;

            using (var proxy = new HttpClient() { BaseAddress = new Uri(_companionService) })
            {
                locations = proxy.GetAsync("Locations?$orderby=Country,City").Result.Content.
                    ReadAsAsync<IEnumerable<LocationDTO>>().Result;
            }

            return View(locations);            
        }

        public ActionResult Schedules(int from, int to)
        {
            IEnumerable<FlightWithSchedulesDTO> flights = null;

            using (var proxy = new HttpClient() { BaseAddress = new Uri(_companionService) })
            {                
                flights = proxy.GetAsync(
                    string.Format("Flights?source={0}&destination={1}", from, to)).
                    Result.Content.ReadAsAsync<IEnumerable<FlightWithSchedulesDTO>>().Result;
            }

            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            return PartialView(flights);
        }

        public ActionResult UpdateActualDeparture(int id, int scheduleId, string date, string time)
        {
            DateTime newDeparture = DateTime.Parse(date + " " + time);
            using (var proxy = new HttpClient() { BaseAddress = new Uri(_companionService) })
            {
                FlightScheduleDTO schedule = proxy.GetAsync(
                    string.Format("Flights/{0}?scheduleId={1}", id, scheduleId)).
                    Result.Content.ReadAsAsync<FlightScheduleDTO>().Result;

                schedule.ActualDeparture = newDeparture;

                proxy.PutAsJsonAsync(
                    string.Format("Flights/{0}?scheduleId={1}", id, scheduleId),
                    schedule).Wait();
            }
            return Content("updated");
        }
    }
}
