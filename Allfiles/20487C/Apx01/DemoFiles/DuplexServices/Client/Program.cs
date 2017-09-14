using Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Client";
            Console.WriteLine("Press Enter when the service is ready");
            Console.ReadLine();

            InstanceContext context = new InstanceContext(new StockCallback());
            DuplexChannelFactory<IStock> factory = new DuplexChannelFactory<IStock>(context, "StockEP");
            IStock proxy = factory.CreateChannel();
            proxy.RegisterForQuote("MSFT");

            Console.WriteLine("Waiting for stock updates. Press Enter to stop");
            Console.ReadLine();
        }
    }
}
