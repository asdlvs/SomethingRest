using SomethingRest.Core.MemberImplementations;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace SomethingRest.Core.InterfaceImplementations
{
    public class RestImplementation : BaseImplementation
    {
        protected RestImplementation(Dictionary<Type, IMemberImplementation<MemberInfo>> implementation) : base(implementation)
        {}

        public RestImplementation() : this(new Dictionary<Type, IMemberImplementation<MemberInfo>>
        {
            { Type.GetType("System.Reflection.RuntimeMethodInfo"), new RestMethodImplementation() },
            { Type.GetType("System.Reflection.RuntimePropertyInfo"), new PropertyImplementation() }
        })
        {

        }
    }
}
