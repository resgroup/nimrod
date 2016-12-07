using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Nimrod
{
    public static class TypeExtensions
    {
        public static TypeScriptType ToTypeScript(this Type type) => new TypeScriptType(type);

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
            || typeof(System.Web.Http.Controllers.IHttpController).IsAssignableFrom(type)
            || typeof(System.Web.Http.ApiController).IsAssignableFrom(type)
            || System.Reflection.TypeExtensions.IsAssignableFrom(type, typeof(System.Web.Http.ApiController));



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


        public static bool IsGeneric1DArray(this Type type, Type[] genericArguments)
            => type.IsGenericArray() && genericArguments.Length == 1;


        public static bool IsGenericArray(this Type type)
            => GenericArrayTypes.Contains(type.GetGenericTypeDefinition());

        /// <summary>
        /// Get every types in the inheritance tree of this type,
        /// ie: return type.BaseType recursively.
        /// </summary>
        public static IEnumerable<Type> GetBaseTypes(this Type type) => type.BaseType == null ? new Type[0] :
            new[] { type.BaseType }.Concat(GetBaseTypes(type.BaseType));

    }
}
