using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Tracing;
using System.Net.Http;
using System.Web.Http.Tracing; 

namespace BlueYonder.Companion.Host
{
    public class TraceWriter : ITraceWriter
    {
        public void Trace(HttpRequestMessage request, string category, System.Web.Http.Tracing.TraceLevel level, Action<System.Web.Http.Tracing.TraceRecord> traceAction)
        {
            
        }
    }
}