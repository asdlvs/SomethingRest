using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;

namespace SomethingRest.Core.Content
{
    public class ObjectContentWriter : ContentSourceBase
    {
        public override HttpContent Create(object item)
        {
            var type = item.GetType();
            MediaTypeFormatter formatter = DefineFormatter(this.Accept, type);
            var content = new ObjectContent(type, item, formatter);
            content.Headers.ContentType = MediaTypeHeaderValue.Parse(this.ContentType);
            return content;
        }
    }
}
