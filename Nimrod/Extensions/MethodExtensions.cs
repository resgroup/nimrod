using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Nimrod
{
    public static class MethodExtensions
    {
        /// <summary>
        /// Return method signature in typescript of a C# method
        /// </summary>
        /// <param name="method">The method</param>
        /// <param name="needNamespace">Do we need to add namespace information on types?</param>
        /// <returns></returns>
        public static string GetMethodSignature(this MethodInfo method, bool needNamespace)
        {
            var builder = new StringBuilder();
            builder.Append(method.Name);
            var namespaceInsert = needNamespace ? "Nimrod." : "";
            builder.Append($"(restApi: {namespaceInsert}IRestApi");

            foreach (var methodParameter in method.GetParameters())
            {
                var param = methodParameter.ParameterType.ToTypeScript(needNamespace);
                builder.Append($", {methodParameter.Name}: {param}");
            }
            builder.Append($", config?: {namespaceInsert}IRequestConfig)");
            string returnType;
            if (method.ReturnType.GetGenericArguments().Length == 1 && method.ReturnType.Name.StartsWith("Json"))
            {
                // the return type is generic, is should be something like Json<T>, so the promise will return a T
                var genericArguments = method.ReturnType.GetGenericArguments()[0];
                returnType = genericArguments.ToTypeScript(needNamespace);
            }
            else
            {
                // the return type is not wrapped, assume it is ApiController and use the return type
                returnType = method.ReturnType.ToTypeScript(needNamespace);
            }
            builder.Append($": {namespaceInsert}IPromise<{returnType}>");
            return builder.ToString();
        }
    }
}
