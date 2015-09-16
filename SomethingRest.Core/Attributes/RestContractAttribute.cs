using System;

namespace SomethingRest.Core.Attributes
{
    public class RestContractAttribute : Attribute
    {
        public string Url { get; set; }

        public string Accept { get; set; }

        public string ContentType { get; set; }

        public RestContractAttribute(string url, string accept, string contentType)
        {
            this.Accept = accept;
            this.ContentType = contentType;
            this.Url = url;
        }
    }
}
