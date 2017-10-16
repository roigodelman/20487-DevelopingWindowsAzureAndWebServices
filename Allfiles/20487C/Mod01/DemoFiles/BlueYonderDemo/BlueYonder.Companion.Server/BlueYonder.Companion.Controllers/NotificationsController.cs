using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using BlueYonder.Companion.Entities;
using BlueYonder.Companion.WNS;
using BlueYonder.DataAccess.Interfaces;
using BlueYonder.DataAccess.Repositories;
using BlueYonder.Entities;
using BlueYonder.Companion.WNS.WorkerRole;

namespace BlueYonder.Companion.Controllers
{
    public class NotificationsController : ApiController
    {
        static NotificationsController()
        {
            WNSManager.Authenticate();
        }

        [HttpPost]
        public HttpResponseMessage Register(RegisterNotificationsRequest request)
        {
            MessageHandler.CreateSubscription(request);

            return Request.CreateResponse(HttpStatusCode.Created);
        }
       
    }
}
