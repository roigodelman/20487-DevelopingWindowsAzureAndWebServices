using RequestResponseFlow.Web.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace RequestResponseFlow.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MessageHandlers.Add(new TraceHandler());

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
