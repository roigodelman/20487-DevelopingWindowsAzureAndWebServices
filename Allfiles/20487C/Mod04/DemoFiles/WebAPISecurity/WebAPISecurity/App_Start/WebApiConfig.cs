using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace WebAPISecurity
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MessageHandlers.Add(new AuthenticationMessageHandler());

            config.Formatters.Remove(config.Formatters.JsonFormatter);

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
