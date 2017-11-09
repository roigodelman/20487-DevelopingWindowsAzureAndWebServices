using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace AsynchronousActions.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "{controller}/{id}",
                defaults: new { id = RouteParameter.Optional, controller = "Countries" }
            );

            config.Formatters.Remove(config.Formatters.JsonFormatter);
        }
    }
}
