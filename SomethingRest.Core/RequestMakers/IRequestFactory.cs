using System.Net.Http;

namespace SomethingRest.Core.RequestMakers
{
    public interface IRequestFactory
    {
        IRequestMaker Create(RequestMethod method, HttpClient client);
    }
}
