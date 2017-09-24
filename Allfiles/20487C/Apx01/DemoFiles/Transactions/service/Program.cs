using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.Configuration;
using System.Data.Entity;

namespace Service
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Service";

            var dbInitializer = new BankDatabaseInitializer();
            dbInitializer.InitializeDatabase(new BankContext());

            ServiceHost host = new ServiceHost(typeof(TransferService));
            try
            {
                host.Opening += new EventHandler((o, a) => Console.WriteLine("Service Host Initializing..."));
                host.Opening += new EventHandler((o, a) => Console.WriteLine("Service Hosted Successfully! Press Enter To Stop"));

                host.Open();
            }
            catch (Exception exc)
            {
                Console.WriteLine("Failed to host the service: {0}", exc);
            }

            Console.ReadLine();
            try
            {
                host.Close();
            }
            catch
            {
                host.Abort();
            }
        }
    }
}
