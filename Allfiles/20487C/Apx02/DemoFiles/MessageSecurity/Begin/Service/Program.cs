using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Service;

namespace Service
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceHost host = new ServiceHost(typeof(CalcService));

            host.Open();
            Console.WriteLine("Service is opened. Press Enter to stop");
            Console.ReadLine();
            host.Close();
        }
    }
}
