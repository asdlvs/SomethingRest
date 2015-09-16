using SomethingRest.Core.MethodImplementations;

namespace SomethingRest.Core.InterfaceImplementations
{
    public class RestImplementation : BaseImplementation
    {
        protected RestImplementation(IMethodImplementation implementation) : base(implementation)
        {}

        public RestImplementation() : this(new RestMethodImplementation())
        { }
    }
}
