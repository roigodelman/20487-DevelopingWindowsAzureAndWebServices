using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Threading.Tasks;

namespace CustomOperationInvoker
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
            return null;
        }

        public object Invoke(object instance, object[] inputs, out object[] outputs)
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();
            object result = _previousInvoker.Invoke(instance, inputs, out outputs); 
            timer.Stop();
            Console.WriteLine("Operation took {0} milliseconds to execute");
            return result;
        }

        public IAsyncResult InvokeBegin(object instance, object[] inputs, AsyncCallback callback, object state)
        {
            throw new NotImplementedException();
        }

        public object InvokeEnd(object instance, out object[] outputs, IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        public bool IsSynchronous
        {
            get { return true; }
        }
    }
}
