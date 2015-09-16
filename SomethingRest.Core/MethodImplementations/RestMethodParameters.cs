using System.Collections.Generic;

namespace SomethingRest.Core.MethodImplementations
{
    public class RestMethodParameters
    {
        public string Url { get; set; }

        public string Method { get; set; }

        public string ContentType { get; set; }

        public List<object> Parameters { get; set; }
    }
}
