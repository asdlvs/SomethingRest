using System.Net.Http;
using System.Threading.Tasks;

namespace SomethingRest.Core.RequestMakers
{
    public class PutRequest : Request
    {
        public PutRequest(HttpClient client) : base(client)
        {
        }

        public override Task<HttpResponseMessage> Make(string url, HttpContent content)
        {
            return Client.PutAsync(url, content);
        }
    }
}
