using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SomethingRest.Core.Content
{
    public abstract class ContentSourceBase : IContentReader, IContentWriter
    {
        public string Accept { get; set; }

        public object Read(Task<HttpResponseMessage> response, Type type)
        {
            if (typeof(Task).IsAssignableFrom(type) && type.IsGenericType)
            {
                Type resultType = type.GenericTypeArguments[0];

                var readTask = response.ContinueWith(task => task.Result.Content.ReadAsAsync(resultType, new List<MediaTypeFormatter> { DefineFormatter(this.Accept, resultType) }));

                var continueWith = typeof(Task<object>).GetMethods()
                 .Where(m => m.Name == "ContinueWith" && m.IsGenericMethod)
                 .Select(m => new { Method = m, Parameters = m.GetParameters() })
                 .Where(m => m.Parameters.Length == 1 && m.Parameters[0].ParameterType.GetGenericTypeDefinition() == typeof(Func<,>))
                 .Select(m => new { Method = m.Method, Parameters = m.Parameters[0].ParameterType.GetGenericArguments() })
                 .Single(m => m.Parameters[0].IsGenericType && m.Parameters[0].GetGenericTypeDefinition() == typeof(Task<>))
                 .Method;

                var continueWithGeneric = continueWith.MakeGenericMethod(resultType);

                var convertMethod = this.GetType().GetMethod("Convert", BindingFlags.NonPublic | BindingFlags.Instance).MakeGenericMethod(resultType);
                var convertMethodTask = convertMethod.Invoke(this, null);
                
                return continueWithGeneric.Invoke(readTask, new[] { convertMethodTask });
            }

            return response.Result.Content.ReadAsAsync(type, new List<MediaTypeFormatter> { DefineFormatter(this.Accept, type) }).Result;
        }

        protected Func<Task<object>, TTarget> Convert<TTarget>() where TTarget : class
        {
            return tsk => (TTarget)tsk.Result;
        }

        protected MediaTypeFormatter DefineFormatter(string accept, Type type)
        {
            MediaTypeFormatter formatter;
            switch (accept)
            {
                case "application/xml":
                    formatter = new XmlMediaTypeFormatter();
                    ((XmlMediaTypeFormatter)formatter).SetSerializer(type, new XmlSerializer(type));
                    break;
                case "application/json":
                default:
                    formatter = new JsonMediaTypeFormatter();
                    break;
            }
            return formatter;
        }

        public string ContentType { get; set; }

        public abstract HttpContent Create(object item);
    }
}
