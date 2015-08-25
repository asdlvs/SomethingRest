using System.Threading.Tasks;
using SomethingRest.Core;
using SomethingRest.Core.Attributes;

namespace SomethingRest.Runner
{
    [RestContract("http://localhost:5000/api/", "application/json", "application/json")]
    public interface ITestInterface
    {
        [RestMethod("values/{id}")]
        Task<Product> Get(int id);

        [RestMethod("values", RequestMethod.Post)]
        int Post(Product value);

        [RestMethod("values", RequestMethod.Put)]
        void Put(int id, Product value);

        [RestMethod("values", RequestMethod.Delete)]
        void Delete(int id);
    }
}
