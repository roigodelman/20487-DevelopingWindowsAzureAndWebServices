using System;
using System.ServiceModel;
using BlueYonder.DataAccess;

namespace BlueYonder.BookingService.Host
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Server Database Is Initializing...");
            var dbInitializer = new FlightScheduleDatabaseInitializer();
            dbInitializer.InitializeDatabase(new TravelCompanionContext(Implementation.BookingService.ConnectionName));

            var host = new ServiceHost(typeof(Implementation.BookingService));

            host.Opening += OnServiceOpening;
            host.Opened += OnServiceOpened;

            try
            {
                host.Open();
            }
            catch (Exception e)
            {
                host = null;
                Console.WriteLine("  ***   Error occured while trying to open the booking service host *** \n\n{0}", e.Message);
                Console.WriteLine("\n\n Press [ENTER] to exit.");
            }

            Console.ReadLine();
            if (host == null) return;

            try
            {
                host.Close();
            }
            catch (Exception)
            {
                host.Abort();
            }
        }


        private static void OnServiceOpened(object sender, EventArgs e)
        {
            Console.WriteLine("Booking Service Is Running... Press [ENTER] to close.");
        }

        private static void OnServiceOpening(object sender, EventArgs e)
        {
            Console.WriteLine("Booking Service Is Initializing...");
        }
    }
}
