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

            // WARNING: This code is only needed for test certificates that cannot be validated            
            ServicePointManager.ServerCertificateValidationCallback = IgnoreCertValidation;

            // Create a proxy
            Service.CalcClient proxy = new Service.CalcClient();
            int result = proxy.Add(1, 2);
            Console.WriteLine("1+2=" + result);
            Console.ReadLine();
        }

        static bool IgnoreCertValidation(object sender, System.Security.Cryptography.X509Certificates.X509Certificate cert, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors error)
        {
            return true;
        }
    }


}



