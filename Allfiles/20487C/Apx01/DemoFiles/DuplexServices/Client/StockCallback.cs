using Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class StockCallback : IStockCallback
    {
        public void StockUpdated(string stockId, float value)
        {
            Console.WriteLine("{0} = {1}", stockId, value);
        }
    }
}
