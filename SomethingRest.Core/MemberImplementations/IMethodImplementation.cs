using System.Reflection;
using System.Reflection.Emit;

namespace SomethingRest.Core.MemberImplementations
{
    public interface IMemberImplementation<in T> where T : MemberInfo
    {
        void Implement(TypeBuilder typeBuilder, T member);
    }
}
