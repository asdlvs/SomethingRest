using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace SomethingRest.Core.Content
{
    interface IContentReader
    {
        string Accept { get; set; }

        object Read(Task<HttpResponseMessage> response, Type type);
    }
}
