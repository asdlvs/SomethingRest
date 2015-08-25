using System.Threading.Tasks;
using SomethingRest.Core;
using SomethingRest.Core.Attributes;

namespace SomethingRest.Runner
{
    [RestContract("http://localhost:5000/api/", "application/json", "application/json")]
    public interface ITestInterface
    {
        [RestMethod("values/{id}")]
        string Test1(int id);

        [RestMethod("values", RequestMethod.Post, contentType: "application/xml")]
        int Post(string value);

        [RestMethod("values", RequestMethod.Put)]
        void Put(int id, string value);

        [RestMethod("values", RequestMethod.Delete)]
        void Delete(int id);

        [RestMethod("values/products", RequestMethod.Post)]
        int PostProduct(Product product);
    }
}
