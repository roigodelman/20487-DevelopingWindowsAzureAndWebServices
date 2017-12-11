using System;
using System.ServiceModel;
using System.Net;
using System.Threading;

namespace Client
{
    class Client
    {
        static void Main()
        {
            // Shrink console window to half size of the screen (better adjusted to lower resolutions)
            Console.SetWindowSize(Console.LargestWindowWidth / 2, Console.LargestWindowHeight / 2);
            Console.SetBufferSize(Console.LargestWindowWidth / 2, Console.BufferHeight);

            // Create a proxy
            Service.CalcClient proxy = new Service.CalcClient();

            // TODO: Set the proxy with a valid username/password credentials

            try
            {
                int result1 = proxy.Add(1, 2);
                Console.WriteLine("1+2=" + result1);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Add failed: " + ex.Message);
            }

            try
            {
                int result2 = proxy.Mul(1, 2);
                Console.WriteLine("1*2=" + result2);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Mul failed: " + ex.Message);
            }

            try
            {
                int result3 = proxy.Sub(1, 2);
                Console.WriteLine("1-2=" + result3);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Sub failed: " + ex.Message);
            }

            try
            {
                int result4 = proxy.Div(1, 2);
                Console.WriteLine("1/2=" + result4);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Div failed: " + ex.Message);
            }
            Console.ReadLine();
        }
    }
}



