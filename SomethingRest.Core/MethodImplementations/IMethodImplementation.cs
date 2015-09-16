using System.Reflection;
using System.Reflection.Emit;

namespace SomethingRest.Core.MethodImplementations
{
    public interface IMethodImplementation
    {
        void Implement(TypeBuilder typeBuilder, MethodInfo method);
    }
}
