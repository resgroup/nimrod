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
            var returnType = method.GetReturnType();
            builder.Append($": {namespaceInsert}IPromise<{returnType.ToTypeScript(needNamespace)}>");
            return builder.ToString();
        }

        /// <summary>
        /// Return method return's type of a controller
        /// It can be the type itself, or the containing type
        /// if it starts with 'Json' string
        /// Example : JsonNetResult'SomeObject' will return SomeObject
        /// </summary>
        /// <param name="method">The method</param>
        /// <returns></returns>
        public static Type GetReturnType(this MethodInfo method)
        {
            var returnType = method.ReturnType;
            if (returnType.Name.StartsWith("Json") && returnType.GetGenericArguments().Length == 1)
            {
                // the return type is generic, is should be something like Json<T>, so the promise will return a T
                var type = returnType.GetGenericArguments()[0];
                return type;
            }
            else
            {
                // the return type is not wrapped, assume it is ApiController and use the return type
                return returnType;
            }
        }
    }
}
