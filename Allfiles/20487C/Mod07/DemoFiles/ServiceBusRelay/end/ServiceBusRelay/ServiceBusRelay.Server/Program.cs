using ServiceBusRelay.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceBus;

namespace ServiceBusRelay.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = new ServiceHost(typeof(ConsoleService), new Uri("sb://BlueYonderServerDemo07YourInitials.servicebus.windows.net"));
            var endpoint = host.AddServiceEndpoint(typeof(IConsoleService), new NetTcpRelayBinding(), "console");
            endpoint.Behaviors.Add(new TransportClientEndpointBehavior
            {
                TokenProvider = TokenProvider.CreateSharedAccessSignatureTokenProvider("RootManageSharedAccessKey", "[access_key]")
            });

            host.Open();

            Console.WriteLine("The server is running");
            Console.ReadKey();
            host.Close();
        }
    }
}
