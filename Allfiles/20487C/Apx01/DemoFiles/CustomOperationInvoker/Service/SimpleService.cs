using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading;

namespace Service
{
    public class SimpleService : ISimpleService
    {
        [TimingOperationBehavior]
        public string PerformLengthyTask()
        {
            Random rnd = new Random();
            Thread.Sleep(rnd.Next(1000, 3000));
            return "Done";
        }
    }
}
