using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Nimrod
{
    public static class TypeExtensions
    {
        public static readonly Type[] NumberTypes = { typeof(short), typeof(int), typeof(long), typeof(float), typeof(double), typeof(decimal) };

        public static readonly Dictionary<Type, HttpMethodAttribute> TypeToHttpMethodAttribute = new Dictionary<Type, HttpMethodAttribute> {
            { typeof(System.Web.Mvc.HttpGetAttribute), HttpMethodAttribute.Get },
            { typeof(System.Web.Http.HttpGetAttribute), HttpMethodAttribute.Get },
            { typeof(System.Web.Mvc.HttpPostAttribute), HttpMethodAttribute.Post },
            { typeof(System.Web.Http.HttpPostAttribute), HttpMethodAttribute.Post },
            { typeof(System.Web.Mvc.HttpPutAttribute), HttpMethodAttribute.Put },
            { typeof(System.Web.Http.HttpPutAttribute), HttpMethodAttribute.Put },
            { typeof(System.Web.Mvc.HttpDeleteAttribute), HttpMethodAttribute.Delete },
            { typeof(System.Web.Http.HttpDeleteAttribute), HttpMethodAttribute.Delete },
        };

        public static HttpMethodAttribute? FirstOrDefaultHttpMethodAttribute(this MethodInfo method)
        {
            foreach (var attribute in method.GetCustomAttributes(true))
            {
                var attributeType = attribute.GetType();
                HttpMethodAttribute enumAttribute;
                if (TypeToHttpMethodAttribute.TryGetValue(attributeType, out enumAttribute))
                {
                    return enumAttribute;
                }
            }
            return null;
        }

        public static bool IsController(this Type type)
        {
            if (typeof(System.Web.Mvc.Controller).IsAssignableFrom(type))
            {
                return true;
            }
            if (typeof(System.Web.Http.Controllers.IHttpController).IsAssignableFrom(type))
            {
                return true;
            }
            return false;
        }

        public static bool IsBuiltinType(this Type type)
        {
            if (type == typeof(string))
            {
                return true;
            }
            if (type.IsNumber())
            {
                return true;
            }
            if (type == typeof(bool))
            {
                return true;
            }
            if (type == typeof(DateTime))
            {
                return true;
            }
            return false;
        }
        public static bool IsSystem(this Type type)
        {
            return type.Namespace.StartsWith("System")
                || type.Namespace.StartsWith("Microsoft")
                || type.IsBuiltinType();
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

            if (type.IsController())
            {
                name = $"{type.Name.Replace("Controller", "Service")}";
            }

            return $"{type.Namespace}.{name}.ts";
        }

        /// <summary>
        /// Return every Type detected by Generics, so inside the <>
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
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

        static public string ToTypeScript(this Type inType)
        {
            return inType.ToTypeScript(false);
        }
        static public string ToTypeScript(this Type inType, bool includeNamespace)
        {
            return inType.ToTypeScript(includeNamespace, true);
        }
        static public string ToTypeScript(this Type type, bool includeNamespace, bool includeGenericArguments)
        {
            string simpleTypeScript;
            if (TryGetTypeScriptForSimpleType(type, includeNamespace, includeGenericArguments, out simpleTypeScript))
            {
                return simpleTypeScript;
            }
            else if (type.IsGenericParameter)
            {
                return type.Name;
            }
            else if (!type.IsGenericType)
            {
                string iPrefix = type.IsEnum ? "" : "I";
                string typeName = $"{iPrefix}{type.Name}";
                return includeNamespace ? $"{type.Namespace}.{typeName}" : $"{typeName}";
            }
            else
            {
                return ToTypeScriptForGenericClass(type, includeNamespace, includeGenericArguments);
            }
        }

        /// <summary>
        /// Return TypeScript type for simple type like numbers, datetime, string, etc..
        /// </summary>
        /// <param name="type"></param>
        /// <param name="includeNamespace"></param>
        /// <param name="includeGenericArguments"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private static bool TryGetTypeScriptForSimpleType(this Type type, bool includeNamespace, bool includeGenericArguments, out string value)
        {
            var options = new Dictionary<Predicate<Type>, Func<Type, bool, bool, string>> {
                { t => t.IsArray(), (t, n, g) => ArrayToTypeScript(t, n)},
                { t => t.IsTuple(), (t, n, g) => TupleToTypeScript(t, n, g) },
                { t => t.IsString(), (t, n, g) => "string" },
                { t => t.IsNumber(), (t, n, g) => "number" },
                { t => t.IsBoolean(), (t, n, g) => "boolean" },
                { t => t.IsDateTime(), (t, n, g) => "Date" },
                { t => t.IsObject(), (t, n, g) => "any" },
                { t => t.IsVoid(), (t, n, g) => "void" }
            };

            foreach (var option in options)
            {
                if (option.Key(type))
                {
                    value = option.Value(type, includeNamespace, includeGenericArguments);
                    return true;
                }
            }
            value = "";
            return false;
        }

        private static readonly HashSet<Type> GenericTupleTypes = new HashSet<Type>(new[]
        {
            typeof(Tuple<>),
            typeof(Tuple<,>),
            typeof(Tuple<,,>),
            typeof(Tuple<,,,>),
            typeof(Tuple<,,,,>),
            typeof(Tuple<,,,,,>),
            typeof(Tuple<,,,,,,>),
            typeof(Tuple<,,,,,,,>)
        });
        public static bool IsTuple(this Type type)
        {
            var typeDefinition = type.IsGenericType ? type.GetGenericTypeDefinition() : type;
            return GenericTupleTypes.Contains(typeDefinition);
        }
        public static bool IsArray(this Type type)
        {
            return type.IsArray;
        }
        public static bool IsString(this Type type)
        {
            return type == typeof(string);
        }

        /// <summary>
        /// Return if the type is strictly equals to System.Object
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsObject(this Type type)
        {
            return type.UnderlyingSystemType.FullName == "System.Object";
        }
        public static bool IsVoid(this Type type)
        {
            return type == typeof(void);
        }
        public static bool IsDateTime(this Type type)
        {
            return type == typeof(DateTime) || type == typeof(DateTimeOffset);
        }
        public static bool IsBoolean(this Type type)
        {
            return type == typeof(bool);
        }
        public static string TupleToTypeScript(this Type type)
        {
            return type.TupleToTypeScript(false);
        }
        public static string TupleToTypeScript(this Type type, bool includeNamespace)
        {
            return type.TupleToTypeScript(includeNamespace, true);
        }
        public static string TupleToTypeScript(this Type type, bool includeNamespace, bool includeGenericArguments)
        {
            // search the generic type definition so the method work for both Tuple<int> and Tuple<T>
            var genericTypeDefinition = type.IsGenericType ? type.GetGenericTypeDefinition() : type;
            if (!genericTypeDefinition.IsTuple())
            {
                throw new ArgumentException(nameof(type), "Type should be a Tuple");
            }
            var content = string.Join(", ", type.GetGenericArguments()
                .Select(a => a.ToTypeScript(includeNamespace, includeGenericArguments))
                .Select((s, i) => $"Item{i + 1}: {s}").ToArray());

            return $"{{ {content} }}";
        }

        public static string ArrayToTypeScript(this Type type, bool includeNamespace)
        {
            var elementTypeName = type.GetElementType().ToTypeScript(includeNamespace);
            return $"{elementTypeName}[]";
        }

        /// <summary>
        /// Complicated stuff for returning the name of a generic class
        /// </summary>
        /// <param name="type"></param>
        /// <param name="includeNamespace"></param>
        /// <param name="includeGenericArguments"></param>
        /// <returns></returns>
        static private string ToTypeScriptForGenericClass(Type type, bool includeNamespace, bool includeGenericArguments)
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
                    var keyType = genericArguments[0];
                    string keyTypescript;
                    // we could allow only string or number for today
                    // we plan to support enum indexer type when typescript will allow
                    // please consider upvote the proposal : https://github.com/Microsoft/TypeScript/issues/2491
                    if (keyType.IsString() || keyType.IsEnum)
                    {
                        keyTypescript = "string";
                    }
                    else
                    {
                        keyTypescript = "number";
                    }
                    var valueTypescript = genericArguments[1].ToTypeScript(includeNamespace);

                    return $"{{ [id: {keyTypescript}] : {valueTypescript}; }}";
                }

                return GenericTypeToTypeScript(type, includeNamespace, includeGenericArguments);
            }
        }
        /// <summary>
        /// Generic type, emit GenericType`2<A, B> as GenericType<A, B> 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="includeNamespace"></param>
        /// <param name="includeGenericArguments"></param>
        /// <returns></returns>
        private static string GenericTypeToTypeScript(Type type, bool includeNamespace, bool includeGenericArguments)
        {
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
                    if (!first)
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

        public static string TypeScriptModuleName(this Type type)
        {
            var fullTypeName = type.ToTypeScript(true, false);
            var index = fullTypeName.LastIndexOf(".I", StringComparison.InvariantCulture) + 1;
            var moduleName = fullTypeName.Substring(0, index) + fullTypeName.Substring(index + 1);
            return moduleName;
        }

    }
}
