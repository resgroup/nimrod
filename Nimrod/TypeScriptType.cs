using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nimrod
{
    public class TypeScriptType
    {
        public string Name => this.Type.Name;
        public string Namespace => this.Type.Namespace;

        public Type Type { get; }
        public TypeScriptType(Type type)
        {
            this.Type = type.ThrowIfNull(nameof(type));
        }


        public override string ToString() => this.ToString((p) => false);

        public string ToString(ToTypeScriptOptions options)
            => this.ToString(options.IncludeNamespace, options.IncludeGenericArguments, options.Nullable);
        public string ToString(Predicate<Type> includeNamespace) => this.ToString(includeNamespace, true);
        public string ToString(Predicate<Type> includeNamespace, bool includeGenericArguments) => this.ToString(includeNamespace, includeGenericArguments, true);
        public string ToString(Predicate<Type> includeNamespace, bool includeGenericArguments, bool strictNullCheck)
        {

            var options = new ToTypeScriptOptions(includeNamespace, includeGenericArguments, strictNullCheck);
            string strictNullCheckString = " | null";
            string simpleTypeScript;
            string result;
            if (TryGetTypeScriptForSimpleType(this.Type, options, out simpleTypeScript))
            {
                result = simpleTypeScript;
            }
            else if (this.Type.IsGenericParameter)
            {
                result = this.Type.Name;
            }
            else if (!this.Type.IsGenericType)
            {
                if (this.Type.IsSystem())
                {
                    result = "{}";
                }
                else
                {
                    string ns = includeNamespace(this.Type) ? $"{this.Type.Namespace.Replace('.', '_')}." : "";
                    result = $"{ns}{this.Type.Name}";
                }
            }
            else
            {
                // nullable is a very specific case
                // is is a value type serialize has a null, this is the only value type that can became null
                var genericArguments = this.Type.GetGenericArguments();
                if (this.Type.FullName != null && this.Type.FullName.Contains("System.Nullable") && genericArguments.Length == 1)
                {
                    result = $"{genericArguments[0].ToTypeScript().ToString(includeNamespace)}{(strictNullCheck ? strictNullCheckString : "")}";
                }
                else
                {
                    result = $"{ToTypeScriptForGenericClass(this.Type, options)}";
                    if (this.Type.IsArray)
                    {
                        result = $"{result}{(strictNullCheck ? strictNullCheckString : "")}";
                    }
                }
            }

            if (strictNullCheck)
            {
                // value type cannot be null
                // except arrays and nullable, which was handle previously
                if (this.Type.IsValueType)
                {
                    return result;
                }
                else
                {
                    if (result.HasNonEmbededWhiteSpace())
                    {
                        return $"({result}){strictNullCheckString}";
                    }
                    else
                    {
                        return $"{result}{strictNullCheckString}";
                    }
                }
            }
            else
            {
                return result;
            }
        }


        private string ArrayToTypeScript(ToTypeScriptOptions options)
        {
            var tsSubType = $"{this.Type.GetElementType().ToTypeScript().ToString(options)}";
            return tsSubType.HasNonEmbededWhiteSpace() ? $"({tsSubType})[]" : $"{tsSubType}[]";
        }

        private static string TupleToTypeScript(Type type, ToTypeScriptOptions options)
        {
            // search the generic type definition so the method work for both Tuple<int> and Tuple<T>
            var genericTypeDefinition = type.IsGenericType ? type.GetGenericTypeDefinition() : type;
            if (!genericTypeDefinition.IsTuple())
            {
                throw new ArgumentException(nameof(type), "Type should be a Tuple");
            }
            var content = type.GetGenericArguments()
                .Select(a => a.ToTypeScript().ToString(options))
                .Select((s, i) => $"Item{i + 1}: {s}")
                .Join(", ");

            return $"{{ {content} }}";
        }

        /// <summary>
        /// Return TypeScript type for simple type like numbers, datetime, string, etc..
        /// </summary>
        /// <param name="type"></param>
        /// <param name="includeNamespace"></param>
        /// <param name="includeGenericArguments"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private static bool TryGetTypeScriptForSimpleType(Type type, ToTypeScriptOptions options, out string value)
        {
            value = PredicateToTypescriptStringMap
                        .Where(kvp => kvp.Key(type))
                        .Select(kvp => kvp.Value(type, options))
                        .FirstOrDefault();
            return value != null;
        }

        // Map predicate on a type to its representation in typescript
        // usage of Dictionary is to simplify the type inference
        // use list of tuples when real tuples will be there in C#7
        private static readonly Dictionary<Predicate<Type>, Func<Type, ToTypeScriptOptions, string>> PredicateToTypescriptStringMap = new Dictionary<Predicate<Type>, Func<Type, ToTypeScriptOptions, string>> {
                { t => t.IsArray, (t, options) => t.ToTypeScript().ArrayToTypeScript(options)},
                { t => t.IsTuple(), (t,options) => TupleToTypeScript(t, options) },
                { t => t == typeof(string), (t, options) => "string" },
                { t => t.IsNumber(), (t, options) => "number" },
                { t => t == typeof(bool), (t,options) => "boolean" },
                { t => t.IsDateTime(), (t, options) => "Date" },
                { t => t.IsObject(), (t, options) => "any" },
                { t => t == typeof(void), (t, options) => "void" }
            };



        /// <summary>
        /// Complicated stuff for returning the name of a generic class
        /// </summary>
        private static string ToTypeScriptForGenericClass(Type type, ToTypeScriptOptions options)
        {
            var genericArguments = type.GetGenericArguments();

            if (type.IsGeneric1DArray(genericArguments))
            {
                string baseType = genericArguments[0].ToTypeScript().ToString(options);
                if (baseType.HasNonEmbededWhiteSpace())
                {
                    return $"({baseType})[]";
                }
                else
                {
                    return $"{baseType}[]";
                }
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
                var valueTypescript = genericArguments[1].ToTypeScript().ToString(options);

                return $"{{ [id: {keyTypescript}] : {valueTypescript}; }}";
            }
            else
            {
                return GenericTypeToTypeScript(type, options);
            }
        }
        /// <summary>
        /// Generic type, emit GenericType`2<A, B> as GenericType<A, B> 
        /// </summary>
        private static string GenericTypeToTypeScript(Type type, ToTypeScriptOptions options)
        {
            var result = new StringBuilder();
            if (options.IncludeNamespace(type))
            {
                result.Append($"{type.GetGenericTypeDefinition().Namespace.Replace('.', '_')}.");
            }

            var genericTypeDefinitionName = type.GetGenericTypeDefinition().Name;
            // generics type got a name like Foo`2, we parse only before the `
            var withoutAfterBacktick = genericTypeDefinitionName.Remove(genericTypeDefinitionName.IndexOf('`'));
            result.Append(withoutAfterBacktick);
            if (options.IncludeGenericArguments)
            {
                var args = type.GetGenericArguments()
                    .Select(t => t.ToTypeScript().ToString(options.IncludeNamespace, true, false))
                    .Join(", ");
                result.Append($"<{args}>");
            }
            return result.ToString();
        }

    }
}
