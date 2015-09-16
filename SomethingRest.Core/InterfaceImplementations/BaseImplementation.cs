using SomethingRest.Core.MethodImplementations;
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
        private readonly IMethodImplementation _imlementation;

        protected BaseImplementation(IMethodImplementation implementation)
        {
            if (implementation == null) { throw new ArgumentNullException("implementation"); }

            _imlementation = implementation;
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

            foreach (var interfaceMethod in interfaceType.GetMethods())
            {
                _imlementation.Implement(typeBuilder, interfaceMethod);
            }

            foreach(var propertyMethod in interfaceType.GetProperties())
            {
                FieldBuilder fieldBuilder = typeBuilder.DefineField(propertyMethod.Name.ToLower(), propertyMethod.PropertyType, FieldAttributes.Private);

                PropertyBuilder propBuilder = typeBuilder.DefineProperty(propertyMethod.Name, PropertyAttributes.None, propertyMethod.PropertyType, null);
                MethodAttributes getSetAttr = MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig | MethodAttributes.Virtual;

                var method = interfaceType.GetMethods().Where(m => m.Name == "get_" + propertyMethod.Name).Single();

                MethodBuilder getMethodBuilder = typeBuilder.DefineMethod("get_" + propertyMethod.Name, getSetAttr, propertyMethod.PropertyType, Type.EmptyTypes);
                ILGenerator getPropertyIlGen = getMethodBuilder.GetILGenerator();
                getPropertyIlGen.Emit(OpCodes.Ldarg_0);
                getPropertyIlGen.Emit(OpCodes.Ldfld, fieldBuilder);
                getPropertyIlGen.Emit(OpCodes.Ret);

                MethodBuilder setMethodBuilder =  typeBuilder.DefineMethod("set_" + propertyMethod.Name, getSetAttr, null, new Type[] { propertyMethod.PropertyType });
                ILGenerator setPropertyIlGen = setMethodBuilder.GetILGenerator();
                setPropertyIlGen.Emit(OpCodes.Ldarg_0);
                setPropertyIlGen.Emit(OpCodes.Ldarg_1);
                setPropertyIlGen.Emit(OpCodes.Stfld, fieldBuilder);
                setPropertyIlGen.Emit(OpCodes.Ret);

                propBuilder.SetGetMethod(getMethodBuilder);
                propBuilder.SetSetMethod(setMethodBuilder);
            }

            var implementationType = typeBuilder.CreateType();
            var instance = (T)Activator.CreateInstance(implementationType);

            return instance;
        }
    }

}
