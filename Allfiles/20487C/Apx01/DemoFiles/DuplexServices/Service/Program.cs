using Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Service Host";
            StockService serviceInstance = new StockService();
            ServiceHost host = new ServiceHost(serviceInstance);
            
            try
            {
                host.AddServiceEndpoint(
                    typeof(IStock),
                    new NetTcpBinding(),
                    "net.tcp://localhost:8080/stocks");

                host.Open();
                HandleUserInput(serviceInstance);              
            }
            catch (Exception exc)
            {
                Console.WriteLine("Failed to host the service: {0}", exc);
            }
            
            try
            {
                host.Close();
            }
            catch
            {
                host.Abort();
            }
        }

        static void HandleUserInput(StockService instance)
        {
            string inputLine;
            Console.WriteLine("Enter stock name followed by new price. Enter Q to quit.");
            while ((inputLine = Console.ReadLine()).ToUpper() != "Q")
            {
                string[] values = inputLine.Split(' ');
                instance.UpdateStockQuote(values[0], int.Parse(values[1]));
                Console.WriteLine("Enter stock name followed by new price. Enter Q to quit.");
            }
        }
    }
}
