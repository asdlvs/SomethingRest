using System;

namespace SomethingRest.Core.Attributes
{ 
    public class RestMethodAttribute : Attribute
    {
        public string Url { get; set; }
        public RequestMethod Method { get; set; }

        public RestMethodAttribute(string url, RequestMethod method = RequestMethod.Get)
        {
            this.Url = url;
            this.Method = method;
        }
    }
}
