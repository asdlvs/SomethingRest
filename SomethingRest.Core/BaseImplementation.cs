using System;
using System.Reflection;
using System.Reflection.Emit;
using SomethingRest.Core.Attributes;
using SomethingRest.Core.Implementator;
using SomethingRest.Core.InterfaceImplementations;

namespace SomethingRest.Core
{
    public class BaseImplementation : IInterfaceImplementation
    {
        protected Type ContainerType { get; set; }

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

            var restContract = interfaceType.GetCustomAttribute<RestContractAttribute>();

            foreach (var interfaceMethod in interfaceType.GetMethods())
            {
                new DefaultImplementator
                {
                    RestContract = restContract
                }.Implement(typeBuilder, interfaceMethod);
            }

            var implementationType = typeBuilder.CreateType();
            var instance = (T)Activator.CreateInstance(implementationType);

            return instance;
        }
    }

}
