using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScalableAzureApp.Models
{
    public class CountingModel
    {
        public long IncrementValue { get; set; } = 1;
        public long CounterValue { get; set; } = 0;
        public string CurrentInstance { get; set; } = "?";
        public string RoleName { get; set; } = "?";
        //public IList<string> Instances { get; set; } = new List<string>();
    }
}