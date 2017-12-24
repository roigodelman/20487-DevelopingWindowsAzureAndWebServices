using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace BlueYonder.Companion.Host
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "TravelerReservationsApi",
                routeTemplate: "travelers/{travelerId}/{controller}"
            );

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
