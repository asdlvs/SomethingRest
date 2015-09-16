using System;
using System.Net.Http;

namespace SomethingRest.Core.RequestMakers
{
    public class RequestFactory : IRequestFactory
    {
        public IRequestMaker Create(RequestMethod method, HttpClient client)
        {
            switch (method)
            {
                case RequestMethod.Get:
                    return new GetRequest(client);
                case RequestMethod.Delete:
                    return new DeleteRequest(client);
                case RequestMethod.Post:
                    return new PostRequest(client);
                case RequestMethod.Put:
                    return new PutRequest(client);
                default:
                    throw new ArgumentException("Wrong Request method");
            }
        }
    }
}
