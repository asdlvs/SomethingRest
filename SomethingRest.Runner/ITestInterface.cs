using System.Threading.Tasks;
using SomethingRest.Core.Attributes;

namespace SomethingRest.Runner
{
    [RestContract("http://localhost:13455/api/")]
    public interface ITestInterface
    {
        [RestMethod("values/{id}")]
        string Test1(int id);
    }
}
