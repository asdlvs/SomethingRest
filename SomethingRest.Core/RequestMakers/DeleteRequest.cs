using System.Net.Http;
using System.Threading.Tasks;

namespace SomethingRest.Core.RequestMakers
{
    public class DeleteRequest : Request
    {
        public DeleteRequest(HttpClient client) : base(client)
        {
        }

        public override Task<HttpResponseMessage> Make(string url, HttpContent content)
        {
            return Client.DeleteAsync(url);
        }
    }
}
