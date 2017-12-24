using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueYonder.Companion.WNS.Notifications
{
    public abstract class ToastNotificationBase
    {
        public string TextHeading { get; set; }

        public string TextBodyWrap { get; set; }

        public abstract string GetNotificationXML();

        public List<int> TargetClientDevices
        {
            get;
            set;
        }
    }
}
