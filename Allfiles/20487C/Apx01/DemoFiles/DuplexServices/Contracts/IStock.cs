using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    [ServiceContract(CallbackContract = typeof(IStockCallback))]
    public interface IStock
    {
        [OperationContract(IsOneWay = true)]
        void RegisterForQuote(string stockId);

        [OperationContract(IsOneWay = true)]
        void UpdateStockQuote(string stockId, float newValue);
    }

}
