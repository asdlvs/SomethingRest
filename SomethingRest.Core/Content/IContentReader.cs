using System;
using System.Net.Http;

namespace SomethingRest.Core.Content
{
    interface IContentReader
    {
        string Accept { get; set; }

        object Read(HttpContent content, Type type);
    }
}
