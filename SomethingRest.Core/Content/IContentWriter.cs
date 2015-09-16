using System.Collections.Generic;
using System.Net.Http;

namespace SomethingRest.Core.Content
{
    public interface IContentWriter
    {
        string ContentType { get; set; }

        HttpContent Create(object item);
    }
}
