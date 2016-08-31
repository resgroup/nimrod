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
            if (!type.IsController())
            {
                throw new ArgumentOutOfRangeException($"Type {type.Name} MUST extends System.Web.Mvc.Controller or System.Web.Http.IHttpControler", nameof(type));
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

        protected string GetControllerName()
        {
            return this.Type.Name.Replace("Controller", "Service");
        }

        private IEnumerable<string> GetInterface()
        {
            var actions = this.Type.GetControllerActions();
            var signatures = actions.Select(a => a.GetMethodSignature(NeedNameSpace));

            return new[] {
                $"export interface I{GetControllerName()} {{",
                signatures.Select(signature => $"{signature};").JoinNewLine(),
                $"}}"
            };
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

                var entityName = this.Type.Name.Substring(0, this.Type.Name.Length - "Controller".Length);
                var genericArgString = method.GetReturnType().ToTypeScript(NeedNameSpace, true);

                var beautifulParamList = parameters
                            .Select(p => $"{p.Name}: {p.Name}")
                            .Join($",{Environment.NewLine}");
                if (httpVerb == HttpMethodAttribute.Get || httpVerb == HttpMethodAttribute.Delete)
                {
                    yield return "(config || (config = {})).params = {";
                    yield return beautifulParamList;
                    yield return "};";
                    yield return $"return restApi.{httpVerb}<{genericArgString}>('/{entityName}/{method.Name}', config);";
                }
                else
                {
                    yield return "let data = {";
                    yield return beautifulParamList;
                    yield return "};";
                    yield return $"return restApi.{httpVerb}<{genericArgString}>('/{entityName}/{method.Name}', data, config);";
                }
                yield return "}";
            }

            yield return "}";
        }

    }
}
