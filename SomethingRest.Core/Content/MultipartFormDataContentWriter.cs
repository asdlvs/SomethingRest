using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;

namespace SomethingRest.Core.Content
{
    public class MultipartFormDataContentWriter : ContentSourceBase
    {
        public override HttpContent Create(object item)
        {
            var type = item.GetType();
            MediaTypeFormatter formatter = base.DefineFormatter(this.Accept, type);

            var content = new MultipartFormDataContent
            {
                new ObjectContent(type, item, formatter)
            };

            content.Headers.ContentType = MediaTypeHeaderValue.Parse(ContentType);
            return content;
        }
    }
}
