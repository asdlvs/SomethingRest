using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading.Tasks;
using SomethingRest.Core.Attributes;
using SomethingRest.Core.Content;
using SomethingRest.Core.RequestMakers;

namespace SomethingRest.Core.Implementator
{
    public class DefaultImplementator : IMethodImpl
    {
        private const MethodAttributes ImplicitImplementation =
            MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.HideBySig;

        public RestContractAttribute RestContract { get; set; }

        public void Implement(TypeBuilder typeBuilder, MethodInfo method)
        {
            var restMethod = method.GetCustomAttribute<RestMethodAttribute>();
            var isVoid = method.ReturnType == typeof(void);
            var parameters = method.GetParameters().ToList();

            MethodBuilder methodBuilder = typeBuilder.DefineMethod(method.Name,
                ImplicitImplementation,
                method.ReturnType,
                parameters.Select(p => p.ParameterType).ToArray());

            ILGenerator ilGen = methodBuilder.GetILGenerator();
            var dictionaryType = typeof(Dictionary<string, object>);

            ilGen.DeclareLocal(dictionaryType);
            ilGen.DeclareLocal(typeof(CallParameters));

            ilGen.Emit(OpCodes.Newobj, dictionaryType.GetConstructor(new Type[0]));
            ilGen.Emit(OpCodes.Stloc_0);

            for (int i = 0; i < parameters.Count; i++)
            {
                var parameter = parameters[i];
                ilGen.Emit(OpCodes.Ldloc_0);
                ilGen.Emit(OpCodes.Ldstr, parameter.Name);
                ilGen.Emit(OpCodes.Ldarg, i + 1);

                if (!parameter.ParameterType.IsClass)
                {
                    ilGen.Emit(OpCodes.Box, parameter.ParameterType);
                }

                ilGen.Emit(OpCodes.Callvirt, dictionaryType.GetMethod("Add"));
            }

            ilGen.Emit(OpCodes.Newobj, typeof(CallParameters).GetConstructor(new Type[0]));

            SetStringProperty(ilGen, "Url", RestContract.Url + restMethod.Url);
            SetStringProperty(ilGen, "Accept", restMethod.Accept ?? RestContract.Accept);
            SetStringProperty(ilGen, "ContentType", restMethod.ContentType ?? RestContract.ContentType);

            ilGen.Emit(OpCodes.Dup);
            ilGen.Emit(OpCodes.Ldc_I4, (int)restMethod.Method);
            ilGen.Emit(OpCodes.Callvirt, typeof(CallParameters).GetMethod("set_Method"));

            ilGen.Emit(OpCodes.Dup);
            ilGen.Emit(OpCodes.Ldloc_0);
            ilGen.Emit(OpCodes.Callvirt, typeof(CallParameters).GetMethod("set_Parameters"));

            ilGen.Emit(OpCodes.Dup);
            ilGen.Emit(OpCodes.Ldtoken, method.ReturnType);
            ilGen.Emit(OpCodes.Call, typeof(Type).GetMethod("GetTypeFromHandle"));
            ilGen.Emit(OpCodes.Callvirt, typeof(CallParameters).GetMethod("set_ReturnType"));

            ilGen.Emit(OpCodes.Stloc_1);
            ilGen.Emit(OpCodes.Ldarg_0);
            ilGen.Emit(OpCodes.Ldloc_1);

            ilGen.Emit(OpCodes.Callvirt, this.GetType().GetMethod("Invoke"));

            if (!method.ReturnType.IsClass)
            {
                ilGen.Emit(OpCodes.Unbox_Any, method.ReturnType);
            }
            else if (isVoid)
            {
                ilGen.Emit(OpCodes.Pop);
            }

            ilGen.Emit(OpCodes.Ret);
        }

        private void SetStringProperty(ILGenerator ilGen, string property, string value)
        {
            ilGen.Emit(OpCodes.Dup);
            ilGen.Emit(OpCodes.Ldstr, value);
            ilGen.Emit(OpCodes.Callvirt, typeof (CallParameters).GetMethod("set_" + property));
        }

        public object Invoke(CallParameters data)
        {
            using (var client = new HttpClient())
            {
                var tuple = MapParametersToUrl(data.Url, data.Parameters);

                var url = tuple.Item1;
                var unprocessedParameters = tuple.Item2;

                var request = new RequestFactory().Create(data.Method, client);
                client.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse(data.Accept));

                var contentSource = new ObjectContentWriter
                {
                    Accept = data.Accept,
                    ContentType = data.ContentType
                };
                HttpContent content = null;
                if (unprocessedParameters.Any())
                {
                    content = contentSource.Create(unprocessedParameters.FirstOrDefault().Value);
                }

                Task<HttpResponseMessage> response = request.Make(url, content);
                var result = contentSource.Read(response.Result.Content, data.ReturnType);
                return result;
            }
        }

        private Tuple<string, Dictionary<string, object>> MapParametersToUrl(string url, Dictionary<string, object> parameters)
        {
            var unprocessedParameters = new Dictionary<string, object>();

            foreach (var pair in parameters)
            {
                string parameterRoute = $"{{{pair.Key}}}";
                string value = pair.Value.ToString();

                if (url.IndexOf(parameterRoute, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    var parameter = pair.Value as ICollection;
                    if (parameter != null)
                    {
                        value = string.Join(",", parameter.Cast<object>());
                    }

                    url = url.Replace(parameterRoute, value);
                }
                else
                {
                    unprocessedParameters.Add(pair.Key, pair.Value);
                }
            }

            return Tuple.Create(url, unprocessedParameters);
        }
    }
}
