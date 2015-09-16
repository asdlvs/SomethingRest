using System.Net.Http;
using System.Threading.Tasks;

namespace SomethingRest.Core.RequestMakers
{
    public class PostRequest : Request
    {
        public PostRequest(HttpClient client) : base(client)
        {
        }

        public override Task<HttpResponseMessage> Make(string url, HttpContent content)
        {
            return Client.PostAsync(url, content);
        }
    }
}
