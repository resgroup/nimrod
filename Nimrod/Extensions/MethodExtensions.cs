using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Nimrod
{
    public static class MethodExtensions
    {
        private static readonly TryGetFunc<Type, HttpMethodAttribute> TypeToHttpMethodAttributeDelegate = (Type attributeType, out HttpMethodAttribute result) =>
        {
            switch (attributeType.Name)
            {
                case "HttpGetAttribute": result = HttpMethodAttribute.Get; return true;
                case "HttpPostAttribute": result = HttpMethodAttribute.Post; return true;
                case "HttpDeleteAttribute": result = HttpMethodAttribute.Delete; return true;
                case "HttpPutAttribute": result = HttpMethodAttribute.Put; return true;
                default: result = HttpMethodAttribute.Get; return false;
            }
        };
        /// <summary>
        /// returns the first Attribut of type HttpMethod found
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public static HttpMethodAttribute? FirstOrDefaultHttpMethodAttribute(this MethodInfo method)
               => method.GetCustomAttributes(true)
                                 .Select(attribute => attribute.GetType())
                                 .ApplyTryGet(TypeToHttpMethodAttributeDelegate)
                                 .FirstOrNullable();

        /// <summary>
        /// Returns the return type and the arguments type of the method
        /// </summary>
        static public IEnumerable<Type> GetReturnTypeAndParameterTypes(this MethodInfo method)
            => new[] { method.GetReturnType() }
                .Union(method.GetParameters().Select(p => p.ParameterType));


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
