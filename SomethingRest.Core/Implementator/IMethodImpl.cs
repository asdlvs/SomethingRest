using System.Reflection;
using System.Reflection.Emit;

namespace SomethingRest.Core.Implementator
{
    public interface IMethodImpl
    {
        void Implement(TypeBuilder typeBuilder, MethodInfo method);
    }
}
