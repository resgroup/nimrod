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
            if (!type.IsWebController())
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
            return headers.Concat(interface_).Concat(implementation).Concat(footer);
        }

        protected string GetControllerName() => this.Type.Name.Replace("Controller", "Service");

        private IEnumerable<string> GetInterface()
        {
            var actions = this.Type.GetWebControllerActions();
            var signatures = actions.Select(a => a.GetMethodSignature(NeedNameSpace));

            return new[] {
                $@"export interface I{GetControllerName()} {{
                    {signatures.Select(signature => $"{signature};").JoinNewLine()}
                }}"
            };
        }

        private IEnumerable<string> GetImplementation()
        {
            var body = TypeDiscovery.GetWebControllerActions(this.Type).SelectMany(method =>
            {
                var httpVerb = method.FirstOrDefaultHttpMethodAttribute();
                var parameters = method.GetParameters();

                var signature = method.GetMethodSignature(NeedNameSpace);

                var entityName = this.Type.Name.Substring(0, this.Type.Name.Length - "Controller".Length);
                var genericArgString = method.GetReturnType().ToTypeScript(NeedNameSpace, true);

                var beautifulParamList = parameters
                            .Select(p => $"{p.Name}: {p.Name}")
                            .Join($",{Environment.NewLine}");

                bool isGetOrDelete = httpVerb == HttpMethodAttribute.Get || httpVerb == HttpMethodAttribute.Delete;
                return new[] {
                    $"public {signature} {{", isGetOrDelete ?
                    $@"(config || (config = {{}})).params = {{
                    {beautifulParamList}
                    }};
                    return restApi.{httpVerb}<{genericArgString}>('/{entityName}/{method.Name}', config);" :

                   $@"let data = {{
                    {beautifulParamList}
                    }};
                    return restApi.{httpVerb}<{genericArgString}>('/{entityName}/{method.Name}', data, config);",
                    $"}}"
                };
            }).JoinNewLine();
            return new[] {
                $"export class {ServiceName} implements I{ServiceName} {{",
                body,
                $"}}"
            };
        }

    }
}
