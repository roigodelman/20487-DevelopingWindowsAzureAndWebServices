using HotelBooking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace ServiceHosting
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Uri tcpURI = new Uri("net.tcp://localhost:8734/");
            
                ServiceHost host = new ServiceHost(typeof(HotelBookingService), tcpURI);

                //TODO: add a service endpoint in code here

                host.Opened += new EventHandler((o, a) => Console.WriteLine("Service Hosted Successfully!"));

                host.Open();
            }
            catch (Exception exc)
            {
                Console.WriteLine("Failed to host the service: {0}", exc);
            }

            Console.ReadLine();
        }
    }
}
