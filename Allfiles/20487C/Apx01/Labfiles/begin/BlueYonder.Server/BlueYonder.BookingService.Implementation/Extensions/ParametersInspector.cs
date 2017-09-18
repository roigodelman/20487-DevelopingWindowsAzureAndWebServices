using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Threading.Tasks;

namespace BlueYonder.BookingService.Implementation.Extensions
{
    class ParametersInspector : IParameterInspector
    {
        public object BeforeCall(string operationName, object[] inputs)
        {
            OperationContext.Current.Extensions.Add(new ParametersInfo(inputs));
            return null;
        }

        public void AfterCall(string operationName, object[] outputs, object returnValue, object correlationState)
        {
            
        }
    }
}
