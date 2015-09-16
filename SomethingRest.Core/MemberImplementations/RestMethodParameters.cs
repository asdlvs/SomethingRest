using System;
using System.Collections.Generic;

namespace SomethingRest.Core
{
    public class CallParameters : IDataContainer
    {
        public string Url { get; set; }

        public RequestMethod Method { get; set; }

        public string Accept { get; set; }

        public string ContentType { get; set; }

        public Type ReturnType { get; set; }

        public Dictionary<string, object> Parameters { get; set; }
    }
}
