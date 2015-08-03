using System.Collections.Generic;

namespace SomethingRest.Core
{
    public class CallParameters : IDataContainer
    {
        public string Url { get; set; }

        public string Method { get; set; }

        public List<object> Parameters { get; set; }
    }
}
