using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Nimrod
{
    public static class TypeDiscovery
    {
        /// <summary>
        /// Get every type referenced by this type,
        /// including Property, Generics and Inheritance
        /// The list is guarranted to be unique
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IEnumerable<Type> EnumerateTypes(Type type)
        {
            return EnumerateTypesRecursive(type, new HashSet<Type>())
                                .Distinct();
        }

        private static IEnumerable<Type> EnumerateTypesRecursive(Type type, HashSet<Type> cache)
        {
            if (!cache.Contains(type))
            {
                cache.Add(type);
                if (type == typeof(string))
                {
                    // string is a reference type, but we don't want to generate property type Length
                    yield return type;
                }
                else
                {
                    yield return type;

                    // generics
                    foreach (var genericArgumentType in type.GetGenericArguments())
                    {
                        yield return genericArgumentType;
                        foreach (var subType in EnumerateTypesRecursive(genericArgumentType, cache))
                        {
                            yield return subType;
                        }
                    }

                    // properties
                    foreach (var property in type.GetProperties())
                    {
                        foreach (var subType in EnumerateTypesRecursive(property.PropertyType, cache))
                        {
                            yield return subType;
                        }
                    }

                    // inheritance
                    foreach (var baseType in GetBaseTypes(type))
                    {
                        yield return baseType;
                        foreach (var subType in EnumerateTypesRecursive(baseType, cache))
                        {
                            yield return subType;
                        }
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
            var actions = GetControllerActions(controller).ToList();
            if (actions.Any())
            {
                // controller are take into account only if at least one action is typescriptable 
                yield return controller;
                foreach (var method in actions)
                {
                    foreach (var actionType in GetControllerActionParameterTypes(method))
                    {
                        foreach (var referencedType in EnumerateTypes(actionType))
                        {
                            yield return referencedType;
                        }
                    }
                }
            }
        }

        static public IEnumerable<Type> GetControllers(IEnumerable<Assembly> assemblies)
        {
            foreach (var assembly in assemblies)
            {
                var controllers = assembly.GetExportedTypes()
                                          .Where(type => type.IsController());
                foreach (var controller in controllers)
                {
                    yield return controller;
                }
            }
        }

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

        /// <summary>
        /// Return the return type and the arguments type of the method
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        static public IEnumerable<Type> GetControllerActionParameterTypes(MethodInfo method)
        {
            var returnType = method.ReturnType.IsGenericType ? method.ReturnType.GetGenericArguments()[0] : method.ReturnType;
            yield return returnType;
            foreach (var parameter in method.GetParameters())
            {
                yield return parameter.ParameterType;
            }
        }

        static public IEnumerable<MethodInfo> GetControllerActions(this Type controllerType)
        {
            if (!controllerType.IsController())
            {
                throw new ArgumentOutOfRangeException($"Type {controllerType.Name} MUST extends System.Web.Mvc.Controller or System.Web.Http.IHttpControler", nameof(controllerType));
            }
            foreach (var method in controllerType.GetMethods())
            {
                var httpVerb = method.FirstOrDefaultHttpMethodAttribute();
                if (httpVerb != null)
                {
                    yield return method;
                }
            }
        }

    }
}
