using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Nimrod
{
    public static class TypeExtensions
    {
        public static readonly HashSet<Type> NumberTypes =
            new[] { typeof(short), typeof(int), typeof(long), typeof(float), typeof(double), typeof(decimal) }
            .ToHashSet();

        public static readonly HashSet<Type> BuiltinTypes =
            new[] { typeof(string), typeof(bool), typeof(DateTime) }.Union(NumberTypes).ToHashSet();

        private static readonly HashSet<Type> TupleTypes = new[]
        {
            typeof(Tuple<>),
            typeof(Tuple<,>),
            typeof(Tuple<,,>),
            typeof(Tuple<,,,>),
            typeof(Tuple<,,,,>),
            typeof(Tuple<,,,,,>),
            typeof(Tuple<,,,,,,>),
            typeof(Tuple<,,,,,,,>)
        }.ToHashSet();

        public static readonly HashSet<Type> GenericArrayTypes = new[] {
                typeof(IEnumerable<>),
                typeof(List<>),
                typeof(IList<>),
                typeof(ICollection<>)
            }.ToHashSet();

        public static bool IsWebController(this Type type)
            => typeof(System.Web.Mvc.Controller).IsAssignableFrom(type)
            || typeof(System.Web.Http.Controllers.IHttpController).IsAssignableFrom(type);


        public static bool IsBuiltinType(this Type type) => BuiltinTypes.Contains(type);
        public static bool IsSystem(this Type type)
            => type.Namespace.StartsWith("System")
            || type.Namespace.StartsWith("Microsoft")
            || type.IsBuiltinType();

        public static bool IsNumber(this Type type) => NumberTypes.Contains(type);


        /// <summary>
        /// Return every Type detected by Generics, so inside the <>
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        static public HashSet<Type> GetGenericArgumentsRecursively(this Type type) => GetGenericArgumentsRecursivelyImpl(type).ToHashSet();
        static private IEnumerable<Type> GetGenericArgumentsRecursivelyImpl(this Type type)
        {
            var genericArguments = type.GetGenericArguments();

            // for each argument in its generics, find recursively its types
            var recursiveSearchResult = genericArguments.SelectMany(t => GetGenericArgumentsRecursivelyImpl(t));

            return recursiveSearchResult.Union(genericArguments);
        }

        static public string ToTypeScript(this Type inType)
            => inType.ToTypeScript(false);
        static public string ToTypeScript(this Type inType, bool includeNamespace)
            => inType.ToTypeScript(includeNamespace, true);
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
                if (type.IsSystem())
                {
                    return "{}";
                }
                else
                {
                    string iPrefix = type.IsEnum ? "" : "I";
                    string typeName = $"{iPrefix}{type.Name}";
                    return includeNamespace ? $"{type.Namespace}.{typeName}" : $"{typeName}";
                }
            }
            else
            {
                return ToTypeScriptForGenericClass(type, includeNamespace, includeGenericArguments);
            }
        }

        // Map predicate on a type to its representation in typescript
        // usage of Dictionary is to simplify the type inference
        // use list of tuples when real tuples will be there in C#7
        private static readonly Dictionary<Predicate<Type>, Func<Type, bool, bool, string>> PredicateToTypescriptStringMap = new Dictionary<Predicate<Type>, Func<Type, bool, bool, string>> {
                { t => t.IsArray, (t, n, g) => t.ArrayToTypeScript(n)},
                { t => t.IsTuple(), (t, n, g) => t.TupleToTypeScript(n, g) },
                { t => t == typeof(string), (t, n, g) => "string" },
                { t => t.IsNumber(), (t, n, g) => "number" },
                { t => t == typeof(bool), (t, n, g) => "boolean" },
                { t => t.IsDateTime(), (t, n, g) => "Date" },
                { t => t.IsObject(), (t, n, g) => "any" },
                { t => t == typeof(void), (t, n, g) => "void" }
            };

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
            value = PredicateToTypescriptStringMap.Where(kvp => kvp.Key(type)).Select(kvp => kvp.Value(type, includeNamespace, includeGenericArguments)).FirstOrDefault();
            return value != null;
        }

        public static bool IsTuple(this Type type)
        {
            var typeDefinition = type.IsGenericType ? type.GetGenericTypeDefinition() : type;
            return TupleTypes.Contains(typeDefinition);
        }


        /// <summary>
        /// Return if the type is strictly equals to System.Object
        /// </summary>
        public static bool IsObject(this Type type)
            => type.UnderlyingSystemType.FullName == "System.Object";


        public static bool IsDateTime(this Type type)
            => type == typeof(DateTime) || type == typeof(DateTimeOffset);

        public static string TupleToTypeScript(this Type type, bool includeNamespace, bool includeGenericArguments)
        {
            // search the generic type definition so the method work for both Tuple<int> and Tuple<T>
            var genericTypeDefinition = type.IsGenericType ? type.GetGenericTypeDefinition() : type;
            if (!genericTypeDefinition.IsTuple())
            {
                throw new ArgumentException(nameof(type), "Type should be a Tuple");
            }
            var content = type.GetGenericArguments()
                .Select(a => a.ToTypeScript(includeNamespace, includeGenericArguments))
                .Select((s, i) => $"Item{i + 1}: {s}")
                .Join(", ");

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
                else if (type.GetGenericTypeDefinition() == typeof(Dictionary<,>))
                {
                    var keyType = genericArguments[0];
                    string keyTypescript;
                    // Today we allow only string or number a for key in a TypeScript Dictionary
                    // we plan to support enum indexer type when typescript will allow
                    // please consider upvote the proposal : https://github.com/Microsoft/TypeScript/issues/2491
                    if (keyType == typeof(string) || keyType.IsEnum)
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
                else
                {
                    return GenericTypeToTypeScript(type, includeNamespace, includeGenericArguments);
                }
            }
        }
        /// <summary>
        /// Generic type, emit GenericType`2<A, B> as GenericType<A, B> 
        /// </summary>
        private static string GenericTypeToTypeScript(Type type, bool includeNamespace, bool includeGenericArguments)
        {
            var result = new StringBuilder();
            if (includeNamespace)
            {
                result.Append($"{type.GetGenericTypeDefinition().Namespace}.");
            }

            var genericTypeDefinitionName = type.GetGenericTypeDefinition().Name;
            // generics type got a name like Foo`2, we parse only before the `
            var withoutAfterBacktick = genericTypeDefinitionName.Remove(genericTypeDefinitionName.IndexOf('`'));
            result.Append($"I{withoutAfterBacktick}");
            if (includeGenericArguments)
            {
                var args = type.GetGenericArguments()
                    .Select(t => t.ToTypeScript(includeNamespace))
                    .Join(", ");
                result.Append($"<{args}>");
            }
            return result.ToString();
        }

        private static bool IsGeneric1DArray(Type type, Type[] genericArguments)
            => type.IsGenericArray() && genericArguments.Length == 1;


        public static bool IsGenericArray(this Type type)
            => GenericArrayTypes.Contains(type.GetGenericTypeDefinition());

        public static string TypeScriptModuleName(this Type type)
        {
            var fullTypeName = type.ToTypeScript(true, false);
            var index = fullTypeName.LastIndexOf(".I", StringComparison.InvariantCulture) + 1;
            var moduleName = fullTypeName.Substring(0, index) + fullTypeName.Substring(index + 1);
            return moduleName;
        }

        /// <summary>
        /// Get every types in the inheritance tree of this type,
        /// ie: return type.BaseType recursively.
        /// </summary>
        public static IEnumerable<Type> GetBaseTypes(this Type type) => type.BaseType == null ? new Type[0] :
            new[] { type.BaseType }.Concat(GetBaseTypes(type.BaseType));

    }
}
