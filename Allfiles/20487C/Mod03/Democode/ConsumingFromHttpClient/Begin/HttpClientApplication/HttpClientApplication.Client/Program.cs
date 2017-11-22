using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace HttpClientApplication.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            CallServer().Wait();
            Console.ReadKey();
        }

        static async Task CallServer()
        {

        }
    }
}