using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class SingletonLoggerExtension : IExtension<ServiceHostBase>
    {
        TextWriter _output;

        public SingletonLoggerExtension(TextWriter output)
        {
            _output = output;
        }

        public void LogMessage(string message)
        {
            _output.WriteLine(message);
        }

        public void Attach(ServiceHostBase owner)
        {
            _output.WriteLine("Logger loaded");
        }

        public void Detach(ServiceHostBase owner)
        {
            _output.WriteLine("Logger unloaded");
        }
    }

}
