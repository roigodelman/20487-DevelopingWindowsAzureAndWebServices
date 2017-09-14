using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading;

namespace CustomOperationInvoker
{
    public class SimpleService : ISimpleService
    {
        public void PerformLengthyTask()
        {
            Thread.Sleep(5000);
        }
    }
}
