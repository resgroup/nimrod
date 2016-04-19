using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using System.Text;

namespace Nimrod
{
    // This project can output the Class library as a NuGet Package.
    // To enable this option, right-click on the project and select the Properties menu item. In the Build tab select "Produce outputs on build".
    public abstract class ControllerToTypeScript : ToTypeScript
    {
        public string ServiceName => this.Type.Name.Replace("Controller", "Service");
        public virtual bool NeedNameSpace => false;

        public ControllerToTypeScript(Type type) : base(type)
        {
            if (!type.IsWebMvcController())
            {
                throw new ArgumentOutOfRangeException($"Type {type.Name} MUST extends System.Web.Mvc.Controller", nameof(type));
            }
        }

        protected abstract IEnumerable<string> GetHeader();
        protected abstract IEnumerable<string> GetFooter();

        public override IEnumerable<string> GetLines()
        {
            var headers = GetHeader();
            var interface_ = GetInterface();
            var implementation = GetImplementation();
            var footer = GetFooter();
            return new[] { headers, interface_, implementation, footer }.SelectMany(s => s);
        }

        private IEnumerable<string> GetInterface()
        {
            var controllerName = this.Type.Name.Replace("Controller", "Service");
            yield return $"export interface I{controllerName} {{";

            var actions = this.Type.GetControllerActions();
            foreach (var method in actions)
            {
                var signature = method.GetMethodSignature(NeedNameSpace);
                yield return $"{signature};";
            }

            yield return "}";
        }

        private IEnumerable<string> GetImplementation()
        {
            yield return $"export class {ServiceName} implements I{ServiceName} {{";

            foreach (var method in TypeDiscovery.GetControllerActions(this.Type))
            {
                var httpVerb = method.FirstOrDefaultHttpMethodAttribute();
                var parameters = method.GetParameters();

                var signature = method.GetMethodSignature(NeedNameSpace);
                yield return $"public {signature} {{";

                string genericArgString = "";
                var genericArguments = method.ReturnType.GetGenericArguments();
                if (genericArguments.Length > 0)
                {
                    genericArgString = $"<{genericArguments[0].ToTypeScript(this.NeedNameSpace)}>";
                }

                var entityName = this.Type.Name.Substring(0, this.Type.Name.Length - "Controller".Length);

                if (httpVerb == HttpMethodAttribute.Get || httpVerb == HttpMethodAttribute.Delete)
                {
                    yield return "(config || (config = {})).params = {";
                    for (int i = 0; i < parameters.Length; i++)
                    {
                        var methodParameter = parameters[i];
                        var needComma = i != parameters.Length - 1;
                        yield return $"{methodParameter.Name}: {methodParameter.Name}{(needComma ? "," : "")}";
                    }
                    yield return "};";
                    yield return $"return restApi.{httpVerb}{genericArgString}('/{entityName}/{method.Name}', config);";
                }
                else
                {
                    yield return "let data = {";
                    for (int i = 0; i < parameters.Length; i++)
                    {
                        var methodParameter = parameters[i];
                        var needComma = i != parameters.Length - 1;
                        yield return $"{methodParameter.Name}: {methodParameter.Name}{(needComma ? "," : "")}";
                    }
                    yield return "};";
                    yield return $"return restApi.{httpVerb}{genericArgString}('/{entityName}/{method.Name}', data, config);";
                }
                yield return "}";
            }

            yield return "}";
        }

    }
}
