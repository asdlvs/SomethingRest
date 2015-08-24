using System;

namespace SomethingRest.Core.Attributes
{
    public class RestContractAttribute : Attribute
    {
        public string Url { get; set; }

        public RestContractAttribute(string url)
        {
            this.Url = url;
        }
    }
}
