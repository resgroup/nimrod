using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Mvc;

namespace Nimrod
{
    public static class TypeExtensions
    {
        public static readonly Type[] NumberTypes = { typeof(short), typeof(int), typeof(long), typeof(float), typeof(double), typeof(decimal) };

        public static HttpMethodAttribute? FirstOrDefaultHttpMethodAttribute(this MethodInfo method)
        {
            foreach (var attribute in method.GetCustomAttributes(true))
            {
                if (attribute is HttpGetAttribute) return HttpMethodAttribute.Get;
                if (attribute is HttpPostAttribute) return HttpMethodAttribute.Post;
                if (attribute is HttpPutAttribute) return HttpMethodAttribute.Put;
                if (attribute is HttpDeleteAttribute) return HttpMethodAttribute.Delete;
                if (attribute is HttpHeadAttribute) return HttpMethodAttribute.Head;
                if (attribute is HttpOptionsAttribute) return HttpMethodAttribute.Options;
                if (attribute is HttpPatchAttribute) return HttpMethodAttribute.Patch;

            }
            return null;
        }

        public static bool IsWebMvcController(this Type type)
        {
            return typeof(Controller).IsAssignableFrom(type);
        }

        public static bool IsBuiltinType(this Type type)
        {
            if (type == typeof(string)) return true;
            if (type.IsNumber()) return true;
            if (type == typeof(bool)) return true;
            if (type == typeof(DateTime)) return true;
            return false;
        }
        public static bool IsSystem(this Type type)
        {
            return type.Namespace.StartsWith("System") || type.IsBuiltinType();
        }
        public static bool IsNumber(this Type type)
        {
            return NumberTypes.Contains(type);
        }
        public static string GetTypeScriptFilename(this Type type)
        {
            string name;
            if (type.IsGenericType)
            {
                var genericType = type.GetGenericTypeDefinition();
                name = genericType.Name.Remove(genericType.Name.IndexOf('`'));
            }
            else
            {
                name = type.Name;
            }

            if (type.IsWebMvcController())
            {
                name = $"{type.Name.Replace("Controller", "Service")}";
            }

            return $"{type.Namespace}.{name}.ts"; ;
        }

        static public IEnumerable<Type> ReferencedTypes(this Type type)
        {
            var seen = new HashSet<Type>();
            foreach (var genericArgument in type.GetGenericArguments())
            {
                if (genericArgument.IsGenericType)
                {
                    if (seen.Add(genericArgument.GetGenericTypeDefinition()))
                    {
                        yield return genericArgument.GetGenericTypeDefinition();
                    }
                }
                else
                {
                    if (seen.Add(genericArgument))
                    {
                        yield return genericArgument;
                    }
                }
                foreach (var referencedType in genericArgument.ReferencedTypes())
                {
                    if (seen.Add(referencedType))
                    {
                        yield return referencedType;
                    }
                }
            }
        }

        static public string ToTypeScript(this Type inType, bool includeNamespace = false, bool includeGenericArguments = true)
        {
            var type = GetType(inType);

            if (type.IsArray)
            {
                return type.GetElementType().ToTypeScript(includeNamespace) + "[]";
            }
            if (type == typeof(string))
            {
                return "string";
            }
            if (type.IsNumber())
            {
                return "number";
            }
            if (type == typeof(bool))
            {
                return "boolean";
            }
            if (type == typeof(DateTime) || type == typeof(DateTimeOffset))
            {
                return "Date";
            }
            if (type.IsGenericParameter)
            {
                return type.Name;
            }
            if (type.IsGenericType == false)
            {
                return includeNamespace ? $"{type.Namespace}.I{type.Name}" : $"I{type.Name}";
            }
            else
            {
                var genericArguments = type.GetGenericArguments();
                if (type.FullName != null && type.FullName.Contains("System.Nullable") && genericArguments.Length == 1)
                {
                    return genericArguments[0].ToTypeScript(includeNamespace);  // optional values
                }
                else
                {
                    if (IsGeneric1DArray(type, genericArguments))
                    {
                        return genericArguments[0].ToTypeScript(includeNamespace) + "[]";
                    }

                    if (type.GetGenericTypeDefinition() == typeof(Dictionary<,>))
                    {
                        var keyTypescript = genericArguments[0].ToTypeScript(includeNamespace);
                        var valueTypescript = genericArguments[1].ToTypeScript(includeNamespace);

                        return $"{{ [id: {keyTypescript}] : {valueTypescript}; }}";
                    }

                    return GenericTypeToTypeScript(type, includeNamespace, includeGenericArguments);
                }
            }
        }

        private static string GenericTypeToTypeScript(Type type, bool includeNamespace, bool includeGenericArguments)
        {
            // Generic type, emit GenericType`2<A, B> as GenericType<A, B>
            var genericTypeName = new StringBuilder();
            if (includeNamespace)
            {
                genericTypeName.Append($"{type.GetGenericTypeDefinition().Namespace}.");
            }
            genericTypeName.Append($"I{type.GetGenericTypeDefinition().Name.TrimEnd("`1234567890".ToCharArray())}");
            if (includeGenericArguments)
            {
                genericTypeName.Append('<');
                bool first = true;
                foreach (var genericArgument in type.GetGenericArguments())
                {
                    if (first == false)
                    {
                        genericTypeName.Append(", ");
                        first = false;
                    }
                    genericTypeName.Append(genericArgument.ToTypeScript(includeNamespace));
                }
                genericTypeName.Append('>');
            }
            return genericTypeName.ToString();
        }

        private static bool IsGeneric1DArray(Type type, Type[] genericArguments)
        {
            return type.IsGenericArray() && genericArguments.Length == 1;
        }

        public static bool IsGenericArray(this Type type)
        {
            return type.GetGenericTypeDefinition() == typeof(IEnumerable<>)
                        || type.GetGenericTypeDefinition() == typeof(List<>)
                        || type.GetGenericTypeDefinition() == typeof(IList<>)
                        || type.GetGenericTypeDefinition() == typeof(ICollection<>);
        }

        private static Type GetType(Type inType)
        {
            if (inType.IsGenericType && inType.FullName == null && !inType.IsSystem())
            {
                return inType.GetGenericTypeDefinition();
            }
            else
            {
                return inType;
            }
        }

        public static string TypeScriptModuleName(this Type type)
        {
            var fullTypeName = type.ToTypeScript(true, false);
            var index = fullTypeName.LastIndexOf(".I") + 1;
            var moduleName = fullTypeName.Substring(0, index) + fullTypeName.Substring(index + 1);
            return moduleName;
        }

    }
}
