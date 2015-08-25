using System;

namespace SomethingRest.Core.Attributes
{ 
    public class RestMethodAttribute : Attribute
    {
        public string Url { get; set; }
        public RequestMethod Method { get; set; }

        public string Accept { get; set; }

        public string ContentType { get; set; }

        public RestMethodAttribute(string url, RequestMethod method = RequestMethod.Get, string accept = null, string contentType = null)
        {
            this.Url = url;
            this.Method = method;
            this.Accept = accept;
            this.ContentType = contentType;
        }
    }
}
