using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Nimrod
{
    public static class AssemblyLocator
    {
        static ConcurrentDictionary<string, Assembly> assemblies = new ConcurrentDictionary<string, Assembly>();

        public static void Init()
        {
            AppDomain.CurrentDomain.AssemblyLoad += new AssemblyLoadEventHandler(CurrentDomain_AssemblyLoad);
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
        }

        static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            var assemblyName = new AssemblyName(args.Name);
            Assembly assembly;
            assemblies.TryGetValue(assemblyName.Name, out assembly);

            // an assembly has been requested somewhere, but we don't load it
            Debug.Assert(assembly != null);
            return assembly;
        }

        static void CurrentDomain_AssemblyLoad(object sender, AssemblyLoadEventArgs args)
        {
            var assembly = args.LoadedAssembly;
            var assemblyName = assembly.GetName();

            // Note that we load assembly by name, not full name
            // This means that we forgot the version number
            // we should handle the version number too,
            // but take into account that we want to deliver the assembly if we don't find the exact same version number
            assemblies.TryAdd(assemblyName.Name, assembly);
        }
    }
}
