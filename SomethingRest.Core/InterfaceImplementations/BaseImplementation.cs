using SomethingRest.Core.MemberImplementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace SomethingRest.Core.InterfaceImplementations
{
    public abstract class BaseImplementation : IInterfaceImplementation
    {
        private readonly Dictionary<Type, IMemberImplementation<MemberInfo>> _imlementations;

        protected BaseImplementation(Dictionary<Type, IMemberImplementation<MemberInfo>> implementations)
        {
            if (implementations == null) { throw new ArgumentNullException("implementation"); }

            _imlementations = implementations;
        }

        public T Implement<T>()
        {
            Type interfaceType = typeof(T);
            if (!interfaceType.IsInterface)
            {
                throw new ArgumentException("Autoimplementation is available only for interfaces");
            }

            AssemblyBuilder assembly = AppDomain.CurrentDomain.DefineDynamicAssembly(new AssemblyName(interfaceType.Name + "Assembly"), AssemblyBuilderAccess.Run);
            ModuleBuilder module = assembly.DefineDynamicModule(interfaceType.Name + "Module");
            TypeBuilder typeBuilder = module.DefineType(interfaceType.Name + "Impl", TypeAttributes.Public | TypeAttributes.AutoClass | TypeAttributes.AnsiClass | TypeAttributes.BeforeFieldInit);
            typeBuilder.AddInterfaceImplementation(interfaceType);

            foreach(var member in interfaceType.GetMembers())
            {
                if (IsProperty(member)) { continue; }

                var implementation = _imlementations[member.GetType()];
                implementation.Implement(typeBuilder, member);
            }

            var implementationType = typeBuilder.CreateType();
            var instance = (T)Activator.CreateInstance(implementationType);

            return instance;
        }

        private bool IsProperty(MemberInfo method)
        {
            string setPrefix = "set_";
            string getPrefix = "get_";

            string methodName = method.Name;;
            bool isSetter = methodName.StartsWith(setPrefix);
            bool isGetter = methodName.StartsWith(getPrefix);

            return isSetter || isGetter;
        }
    }

}
