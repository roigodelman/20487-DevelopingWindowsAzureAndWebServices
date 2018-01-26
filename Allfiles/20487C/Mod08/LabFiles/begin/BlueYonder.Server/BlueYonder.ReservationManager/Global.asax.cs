using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using System.Data.Entity;
using System.Web.Configuration;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using System.Web.Optimization;

namespace BlueYonder.ReservationManager
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            VerifyTopicAndSubscriptions();
        }

        private void VerifyTopicAndSubscriptions()
        {
            // TODO: Exercise 2: Task 2a: Get the service bus connection info
            // from the web.config (should be CloudConfig after deploying to azure)
            // and connect to the namespace

            // Create the topic if it does not exist already
            string connectionString =
                WebConfigurationManager.AppSettings["Microsoft.ServiceBus.ConnectionString"];

            string topicName =
                WebConfigurationManager.AppSettings["Microsoft.ServiceBus.TopicName"];

            var namespaceManager =
                NamespaceManager.CreateFromConnectionString(connectionString);

            // TODO: Exercise 2: Task 2b: Verify the topic exists, and create it if it doesn't

            // Configure Topic Settings
            TopicDescription td = new TopicDescription(topicName);
            td.DefaultMessageTimeToLive = new TimeSpan(0, 10, 0);

            if (!namespaceManager.TopicExists(td.Path))
            {
                namespaceManager.CreateTopic(td);
            }
        }
    }
}