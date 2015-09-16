using System.Net.Http;
using System.Threading.Tasks;

namespace SomethingRest.Core.RequestMakers
{
    public interface IRequestMaker
    {
        Task<HttpResponseMessage> Make(string url, HttpContent content);
    }
}
