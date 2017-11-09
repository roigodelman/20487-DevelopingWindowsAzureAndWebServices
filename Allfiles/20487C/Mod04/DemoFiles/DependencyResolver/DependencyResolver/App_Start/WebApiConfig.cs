using DependencyResolver.Infrastracture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace DependencyResolver
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.DependencyResolver = new ManualDependencyResolver();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
