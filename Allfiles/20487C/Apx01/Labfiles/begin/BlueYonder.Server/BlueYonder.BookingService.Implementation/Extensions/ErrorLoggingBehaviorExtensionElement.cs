using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel.Configuration;


namespace BlueYonder.BookingService.Implementation.Extensions
{
    class ErrorLoggingBehaviorExtensionElement : BehaviorExtensionElement
    {
        protected override object CreateBehavior() => new ErrorLoggingBehavior();

        public override Type BehaviorType => typeof(ErrorLoggingBehavior);
    }
}
