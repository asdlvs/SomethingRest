using System;

namespace SomethingRest.Core.Attributes
{
    public class RestMethodAttribute : Attribute
    {
        public string Path { get; set; }

        public RestMethod Method { get; set; }

        public string ContentType { get; set; }
    }
}
