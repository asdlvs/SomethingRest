using System;
using System.Reflection;
using System.Reflection.Emit;

namespace SomethingRest.Core.MemberImplementations
{
    public class PropertyImplementation : IMemberImplementation<PropertyInfo>
    {
        public void Implement(TypeBuilder typeBuilder, PropertyInfo member)
        {
            FieldBuilder fieldBuilder = typeBuilder.DefineField(member.Name.ToLower(), member.PropertyType, FieldAttributes.Private);

            PropertyBuilder propBuilder = typeBuilder.DefineProperty(member.Name, PropertyAttributes.None, member.PropertyType, null);
            MethodAttributes getSetAttr = MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig | MethodAttributes.Virtual;

            string getPropertyName = "get_" + member.Name;
            MethodBuilder getMethodBuilder = typeBuilder.DefineMethod(getPropertyName, getSetAttr, member.PropertyType, Type.EmptyTypes);
            ILGenerator getPropertyIlGen = getMethodBuilder.GetILGenerator();
            getPropertyIlGen.Emit(OpCodes.Ldarg_0);
            getPropertyIlGen.Emit(OpCodes.Ldfld, fieldBuilder);
            getPropertyIlGen.Emit(OpCodes.Ret);

            string setPropertyName = "set_" + member.Name;
            MethodBuilder setMethodBuilder = typeBuilder.DefineMethod(setPropertyName, getSetAttr, null, new Type[] { member.PropertyType });
            ILGenerator setPropertyIlGen = setMethodBuilder.GetILGenerator();
            setPropertyIlGen.Emit(OpCodes.Ldarg_0);
            setPropertyIlGen.Emit(OpCodes.Ldarg_1);
            setPropertyIlGen.Emit(OpCodes.Stfld, fieldBuilder);
            setPropertyIlGen.Emit(OpCodes.Ret);

            propBuilder.SetGetMethod(getMethodBuilder);
            propBuilder.SetSetMethod(setMethodBuilder);
        }
    }
}
