using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace BlueYonder.BookingService.Implementation.Extensions
{
    class ParametersInfo : IExtension<OperationContext>
    {
        public object[] Parameters { get; set; }

        public ParametersInfo(object[] parameters)
        {
            Parameters = parameters;
        }

        public void Attach(OperationContext owner)
        {
            
        }

        public void Detach(OperationContext owner)
        {
            
        }
    }
}
