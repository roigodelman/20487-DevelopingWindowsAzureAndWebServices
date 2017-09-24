using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class TimingOperationInvoker : IOperationInvoker
    {
        IOperationInvoker _previousInvoker = null;

        public TimingOperationInvoker(IOperationInvoker previousInvoker)
        {
            _previousInvoker = previousInvoker;
        }

        public object[] AllocateInputs()
        {
            return _previousInvoker.AllocateInputs();
        }

        public object Invoke(object instance, object[] inputs, out object[] outputs)
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();
            object result = _previousInvoker.Invoke(instance, inputs, out outputs);
            timer.Stop();

            ServiceHostBase host = OperationContext.Current.Host;
            SingletonLoggerExtension extension = host.Extensions.Find<SingletonLoggerExtension>();
            extension.LogMessage(string.Format("Operation took {0} milliseconds to execute", timer.ElapsedMilliseconds));
            
            return result;
        }

        public IAsyncResult InvokeBegin(object instance, object[] inputs, AsyncCallback callback, object state)
        {
            // Use the default invoker. Don't time async methods
            return _previousInvoker.InvokeBegin(instance, inputs, callback, state);
        }

        public object InvokeEnd(object instance, out object[] outputs, IAsyncResult result)
        {
            // Use the default invoker. Don't time async methods
            return _previousInvoker.InvokeEnd(instance, out outputs, result);
        }

        public bool IsSynchronous
        {
            get { return _previousInvoker.IsSynchronous; }
        }
    }
}
