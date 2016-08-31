using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Nimrod
{
    public static class TypeDiscovery
    {
        /// <summary>
        /// Get every type referenced by this type,
        /// including itself, Properties, Generics and Inheritance
        /// The list is guarranted to be unique
        /// </summary>
        public static HashSet<Type> EnumerateTypes(Type type)
        {
            var types = new HashSet<Type>();
            EnumerateTypesRecursive(type, types);
            return types;
        }

        /// <summary>
        /// Fill the cache object until no more new types are discovered
        /// </summary>
        private static void EnumerateTypesRecursive(Type type, HashSet<Type> cache)
        {
            if (!cache.Contains(type))
            {
                cache.Add(type);
                // string is a reference type, but we don't want to generate property type Length
                if (type != typeof(string))
                {
                    // generics
                    foreach (var genericArgumentType in type.GetGenericArguments())
                    {
                        EnumerateTypesRecursive(genericArgumentType, cache);
                    }

                    // properties
                    foreach (var property in type.GetProperties())
                    {
                        try
                        {
                            EnumerateTypesRecursive(property.PropertyType, cache);
                        }
                        catch (FileNotFoundException fileNotFoundException)
                        {
                            // during reflection, a type could be found without finding is corrsponding DLLs for reference
                            string message = $@"
Cannot understand the property {property.Name} of type {type.FullName}.
The following DLL has not been found in the loaded assemblies: {fileNotFoundException.FileName}
You should check that the DLLs exists in the folder, and version numbers are the sames.
                            ";
                            throw new FileNotFoundException(message, fileNotFoundException.FileName, fileNotFoundException);
                        }
                    }

                    // inheritance
                    foreach (var baseType in GetBaseTypes(type))
                    {
                        EnumerateTypesRecursive(baseType, cache);
                    }

                }
            }
        }

        /// <summary>
        /// Return every Type referenced by the controller
        /// and the controller itself if it has at least one action detected
        /// </summary>
        /// <param name="controller"></param>
        /// <returns></returns>
        public static IEnumerable<Type> SeekTypesFromController(Type controller)
        {
            if (!controller.IsController())
            {
                string message = $"Type {controller.Name} MUST extend System.Web.Mvc.Controller or System.Web.Http.IHttpControler";
                throw new ArgumentOutOfRangeException(message, nameof(controller));
            }
            var referencedType = GetControllerActions(controller).SelectMany(action =>
                           action.GetReturnTypeAndParameterTypes()
                                 .SelectMany(type => EnumerateTypes(type))
                        );
            return new[] { controller }.Union(referencedType);
        }

        static public IEnumerable<Type> GetControllers(IEnumerable<Assembly> assemblies)
            => assemblies.SelectMany(assembly => assembly.GetExportedTypes())
                         .Where(type => type.IsController());


        public static IEnumerable<Type> GetBaseTypes(Type type)
        {
            bool isValid;
            Type loop = type;
            do
            {
                loop = loop.BaseType;
                isValid = loop != null && !loop.IsSystem();
                if (isValid)
                {
                    yield return loop;
                }
            } while (isValid);
        }

        static public IEnumerable<MethodInfo> GetControllerActions(this Type controllerType)
        {
            if (!controllerType.IsController())
            {
                throw new ArgumentOutOfRangeException($"Type {controllerType.Name} MUST extends System.Web.Mvc.Controller or System.Web.Http.IHttpControler", nameof(controllerType));
            }
            return controllerType.GetMethods()
                                 .Where(method => method.FirstOrDefaultHttpMethodAttribute().HasValue);
        }

    }
}
