using SomethingRest.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace SomethingRest.Core.MemberImplementations
{
    public class RestMethodImplementation : IMemberImplementation<MethodInfo>
    {
        private const MethodAttributes ImplicitImplementation =
            MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.HideBySig;

        public void Implement(TypeBuilder typeBuilder, MethodInfo member)
        {
            var parameterTypes = member.GetParameters().Select(parameter => parameter.ParameterType).ToList();

            MethodBuilder methodBuilder = typeBuilder.DefineMethod(member.Name,
                ImplicitImplementation,
                member.ReturnType,
                parameterTypes.ToArray());

            //var restMethod = member.GetCustomAttribute<RestMethodAttribute>();
            var isVoid = member.ReturnType == typeof(void);

            ILGenerator ilGen = methodBuilder.GetILGenerator();
            ilGen.DeclareLocal(typeof(List<object>));
            ilGen.DeclareLocal(typeof(CallParameters));

            ilGen.Emit(OpCodes.Newobj, typeof(List<object>).GetConstructor(new Type[0]));
            ilGen.Emit(OpCodes.Stloc_0);

            for (int i = 0; i < parameterTypes.Count; i++)
            {
                ilGen.Emit(OpCodes.Ldloc_0);
                ilGen.Emit(OpCodes.Ldarg, i + 1);
                ilGen.Emit(OpCodes.Callvirt, typeof(List<object>).GetMethod("Add"));
            }

            ilGen.Emit(OpCodes.Newobj, typeof(CallParameters).GetConstructor(new Type[0]));

            ilGen.Emit(OpCodes.Dup);
            ilGen.Emit(OpCodes.Ldstr, "http://tempuri/");
            ilGen.Emit(OpCodes.Callvirt, typeof(CallParameters).GetMethod("set_Url"));

            ilGen.Emit(OpCodes.Dup);
            ilGen.Emit(OpCodes.Ldstr, "GET");
            ilGen.Emit(OpCodes.Callvirt, typeof(CallParameters).GetMethod("set_Method"));

            ilGen.Emit(OpCodes.Dup);
            ilGen.Emit(OpCodes.Ldstr, "Application/JSON");
            ilGen.Emit(OpCodes.Callvirt, typeof(CallParameters).GetMethod("set_ContentType"));

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

        public object Invoke(CallParameters parameters)
        {
            return parameters;
        }
    }
}
