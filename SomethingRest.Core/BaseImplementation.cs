using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace SomethingRest.Core
{
    public class BaseImplementation : IInterfaceImplementation
    {
        protected Type ContainerType { get; set; }

        private const MethodAttributes ImplicitImplementation =
            MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.HideBySig;

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
                var isVoid = interfaceMethod.ReturnType == typeof(void);
                var parameterTypes = interfaceMethod.GetParameters().Select(parameter => parameter.ParameterType).ToList();

                MethodBuilder methodBuilder = typeBuilder.DefineMethod(interfaceMethod.Name,
                    ImplicitImplementation,
                    interfaceMethod.ReturnType,
                    parameterTypes.ToArray());

                ILGenerator ilGen = methodBuilder.GetILGenerator();
                ilGen.DeclareLocal(typeof (List<object>));
                ilGen.DeclareLocal(typeof (CallParameters));
                
                ilGen.Emit(OpCodes.Newobj, typeof(List<object>).GetConstructor(new Type[0]));
                ilGen.Emit(OpCodes.Stloc_0);

                for (int i = 0; i < parameterTypes.Count; i++)
                {
                    ilGen.Emit(OpCodes.Ldloc_0);
                    ilGen.Emit(OpCodes.Ldarg, i + 1);
                    ilGen.Emit(OpCodes.Callvirt, typeof(List<object>).GetMethod("Add"));
                }

                ilGen.Emit(OpCodes.Newobj, typeof (CallParameters).GetConstructor(new Type[0]));

                ilGen.Emit(OpCodes.Dup);
                ilGen.Emit(OpCodes.Ldstr, "http://tempuri/");
                ilGen.Emit(OpCodes.Callvirt, typeof(CallParameters).GetMethod("set_Url"));

                ilGen.Emit(OpCodes.Dup);
                ilGen.Emit(OpCodes.Ldstr, "GET");
                ilGen.Emit(OpCodes.Callvirt, typeof(CallParameters).GetMethod("set_Method"));

                ilGen.Emit(OpCodes.Dup);
                ilGen.Emit(OpCodes.Ldloc_0);
                ilGen.Emit(OpCodes.Callvirt, typeof(CallParameters).GetMethod("set_Parameters"));

                ilGen.Emit(OpCodes.Stloc_1);
                ilGen.Emit(OpCodes.Ldarg_0);
                ilGen.Emit(OpCodes.Ldloc_1);

                ilGen.Emit(OpCodes.Callvirt, this.GetType().GetMethod("Invoke"));

                if (isVoid)
                {
                    ilGen.Emit(OpCodes.Pop);
                }

                ilGen.Emit(OpCodes.Ret);
            }

            var implementationType = typeBuilder.CreateType();
            var instance = (T)Activator.CreateInstance(implementationType);

            return instance;
        }

        public object Invoke(CallParameters data)
        {
            return data;
        }
    }

}
