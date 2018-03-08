using System;
using System.Web.Mvc;
using ScalableAzureApp.Models;

namespace ScalableAzureApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly GlobalSharedContext _sharedContext = new GlobalSharedContext();

        public ActionResult Index(CountingModel model)
        {
            model.RoleName = Environment.GetEnvironmentVariable("WEBSITE_SITE_NAME");
            string instanceId = Environment.GetEnvironmentVariable("WEBSITE_INSTANCE_ID")?.Substring(0, 6).ToUpper();

            if (string.IsNullOrEmpty(model.RoleName))
                model.RoleName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;

            if (string.IsNullOrEmpty(instanceId))
                instanceId = Environment.MachineName;

            model.CurrentInstance = instanceId;

            ViewBag.Message = $"[{model.RoleName}] [{instanceId}] Your application description page.";

            if (model.IncrementValue == 0)
            {
                model.CounterValue = _sharedContext.GetCounter();
            }
            else
            {
                model.CounterValue = _sharedContext.UpdateCounter(model.IncrementValue);
            }

            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}