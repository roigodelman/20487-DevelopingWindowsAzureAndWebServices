using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Diagnostics;

namespace BlueYonder.BookingService.Implementation.Extensions
{
    class LoggingErrorHandler : IErrorHandler
    {
        private TraceSource _traceSource = new TraceSource("ErrorHandlerTrace");
        public void ProvideFault(Exception error, MessageVersion version, ref Message fault)
        {
            
        }

        public bool HandleError(Exception error)
        {
            ParametersInfo parametersInfo =
                OperationContext.Current.Extensions.Find<ParametersInfo>();
            if (parametersInfo != null)
            {
                string errParams = parametersInfo.Parameters
                    .Select(ErrorLoggingUtils
                    .GetObjectAsXml).Aggregate((prev, next) => $"{prev}\n{next}");
                string message =
                    $"Exception of type {error.GetType().Name} occured: {error.Message}\n operation parameters are:\n{errParams}\n";
                _traceSource.TraceEvent(TraceEventType.Error, 0, message);
            }
            return true;
        }
    }
}
