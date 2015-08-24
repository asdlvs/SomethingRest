using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace SomethingRest.Core.RequestMakers
{
    public abstract class Request : IRequestMaker
    {
        protected HttpClient Client;

        protected Request(HttpClient client)
        {
            if (client == null) { throw new ArgumentNullException(nameof(client)); }

            this.Client = client;
        }

        public abstract Task<HttpResponseMessage> Make(string url, HttpContent content);
    }
}
