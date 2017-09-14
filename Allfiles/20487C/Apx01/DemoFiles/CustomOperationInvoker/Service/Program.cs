using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    class Program
    {
        static void Main(string[] args)
        {
             ServiceHost host = new ServiceHost(
                typeof(SimpleService),
                new Uri("http://localhost:8080/SimpleService"));
            try
            {
                host.Description.Behaviors.Add(new ServiceMetadataBehavior { HttpGetEnabled = true });
                
                host.Extensions.Add(new SingletonLoggerExtension(Console.Out));

                host.Opening += new EventHandler((o, a) => Console.WriteLine("Service Host Initializing..."));
                host.Opening += new EventHandler((o, a) => Console.WriteLine("Service Hosted Successfully! Press Enter To Stop"));

                host.Open();
            }
            catch (Exception exc)
            {
                Console.WriteLine("Failed to host the service: {0}", exc);
            }

            Console.ReadLine();
            host.Extensions.Remove(
                host.Extensions.Find<SingletonLoggerExtension>());
            host.Close();
        }
    }
}
