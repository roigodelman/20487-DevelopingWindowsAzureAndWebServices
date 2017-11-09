using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Routing;
using BlueYonder.Companion.Controllers;
using BlueYonder.Companion.Controllers.Formatters;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;
using BlueYonder.Entities;
using BlueYonder.Companion.Entities;

namespace BlueYonder.Companion.Host
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();
            
            // TODO: Module 4: Exercise 1: Task 3.1: Register the BlueYonder Resolver
            config.DependencyResolver = new BlueYonderResolver();

			config.Formatters.Add(new AtomFormatter()); 
            config.MessageHandlers.Add(new AtomHandler());

            config.Routes.MapHttpRoute(
                name: "atom",
                routeTemplate: "atom/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<LocationDTO>("Locations");
            builder.EntityType<LocationDTO >().Filter("City");
            config.MapODataServiceRoute(
                routeName: "ODataRoute",
                routePrefix: "odata",
                model: builder.GetEdmModel());
            config.EnsureInitialized();
        }
    }
}
