using System.Net.Http;
using System.Threading.Tasks;

namespace SomethingRest.Core.RequestMakers
{
    public class GetRequest : Request
    {
        public GetRequest(HttpClient client) : base(client)
        {
        }

        public override Task<HttpResponseMessage> Make(string url, HttpContent content)
        {
            return Client.GetAsync(url);
        }
    }
}
