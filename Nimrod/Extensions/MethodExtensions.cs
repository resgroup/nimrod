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

            if (needNamespace)
            {
                builder.Append("(restApi: Nimrod.IRestApi");
            }
            else
            {
                builder.Append("(restApi: IRestApi");
            }
            foreach (var methodParameter in method.GetParameters())
            {
                builder.Append(", ");
                builder.Append(methodParameter.Name);
                builder.Append(": ");
                builder.Append(methodParameter.ParameterType.ToTypeScript(needNamespace));
            }
            builder.Append(", config?: Nimrod.IRequestShortcutConfig)");
            string returnType;
            if (method.ReturnType.GetGenericArguments().Length == 1)
            {
                // the return type is generic, is should be something like Json<T>, so the promise will return a T
                var genericArguments = method.ReturnType.GetGenericArguments()[0];
                returnType = genericArguments.ToTypeScript(needNamespace);
            }
            else
            {
                // the return type is not wrapped, we can't determine it, so just a basic object
                returnType = "{}";
            }
            builder.Append($": Nimrod.IPromise<{returnType}>");
            return builder.ToString();
        }
    }
}
