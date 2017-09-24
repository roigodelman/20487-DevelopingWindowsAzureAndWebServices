using Contracts;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    [ServiceBehavior(
        InstanceContextMode = InstanceContextMode.Single,
        ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class StockService : IStock
    {
        Dictionary<string, List<IStockCallback>> _registrations =
            new Dictionary<string, List<IStockCallback>>();

        public void RegisterForQuote(string stockId)
        {
            lock (_registrations)
            {
                if (!_registrations.ContainsKey(stockId))
                {
                    _registrations[stockId] = new List<IStockCallback>();
                }
            }

            IStockCallback callback = OperationContext.Current.GetCallbackChannel<IStockCallback>();
            _registrations[stockId].Add(callback);
        }

        public void UpdateStockQuote(string stockId, float newValue)
        {
            if (_registrations.ContainsKey(stockId))
            {
                List<IStockCallback> callbacks = _registrations[stockId];

                foreach (IStockCallback callback in callbacks)
                {
                    try
                    {
                        // Only call clients that have open channels
                        if ((callback as ICommunicationObject).State == CommunicationState.Opened)
                        {
                            callback.StockUpdated(stockId, newValue);
                        }
                    }
                    catch (Exception)
                    {
                        // This will prevent the callback from being used again
                        (callback as ICommunicationObject).Abort();
                    }
                }                
            }
        }
    }
}
