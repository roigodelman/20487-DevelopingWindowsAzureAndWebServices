using BlueYonder.Companion.Controllers.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace BlueYonder.Companion.Controllers
{
    public class FrequentFlyersController : ApiController
    {
        private ChannelFactory<FrequentFlyerService.Contracts.IFrequentFlyerService> factory =
            new ChannelFactory<FrequentFlyerService.Contracts.IFrequentFlyerService>("FrequentFlyerTcp");

        public FrequentFlyer Get(int id)
        {
            FrequentFlyerService.Contracts.IFrequentFlyerService proxy = factory.CreateChannel();

            int miles = proxy.GetAccumulatedMiles(id);

            return new FrequentFlyer
            {
                Id = id,
                Miles = miles
            };            
        }
    }
}
