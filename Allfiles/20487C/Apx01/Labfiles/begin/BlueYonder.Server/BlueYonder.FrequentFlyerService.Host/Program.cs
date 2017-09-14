using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using BlueYonder.DataAccess;

namespace BlueYonder.FrequentFlyerService.Host
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Server Database Is Initializing...");
            var dbInitializer = new FrequentFlyerDatabaseInitializer();
            dbInitializer.InitializeDatabase(new FrequentFlyersContext(Implementation.FrequentFlyerService.ConnectionName));

            var host = new ServiceHost(typeof(Implementation.FrequentFlyerService));

            host.Opening += OnServiceOpening;
            host.Opened += OnServiceOpened;

            try
            {
                host.Open();
            }
            catch (Exception e)
            {
                host = null;
                Console.WriteLine("  ***   Error occured while trying to open the Fequent flyer service host *** \n\n{0}", e.Message);
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
            Console.WriteLine("Frequent Flyer Service Is Running... Press [ENTER] to close.");
        }

        private static void OnServiceOpening(object sender, EventArgs e)
        {
            Console.WriteLine("Frequent Flyer Service Is Initializing...");
        }
    }
}
