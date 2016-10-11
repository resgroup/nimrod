using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Nimrod.Writers
{
    // This project can output the Class library as a NuGet Package.
    // To enable this option, right-click on the project and select the Properties menu item. In the Build tab select "Produce outputs on build".
    public class ControllerToTypeScript : ToTypeScript
    {
        public override FileType FileType => FileType.Controller;
        public string ServiceName => this.Type.Name.Replace("Controller", "Service");

        public ControllerToTypeScript(TypeScriptType type, bool strictNullCheck)
            : base(type, strictNullCheck)
        {
            if (!type.Type.IsWebController())
            {
                throw new ArgumentOutOfRangeException($"Type {type.Name} MUST extends System.Web.Mvc.Controller or System.Web.Http.IHttpControler", nameof(type));
            }
        }
        public override IEnumerable<Type> GetImports()
        {
            var actions = TypeDiscovery.GetWebControllerActions(this.Type.Type);
            var importedTypes = actions.SelectMany(action => action.GetReturnTypeAndParameterTypes())
                                       .Distinct();

            var imports = ModuleHelper.GetTypesToImport(importedTypes);
            return imports;
        }
        public override IEnumerable<string> GetLines()
        {
            var body = TypeDiscovery.GetWebControllerActions(this.Type.Type).SelectMany(method =>
            {
                var httpVerb = method.FirstOrDefaultHttpMethodAttribute();
                var parameters = method.GetParameters();

                var signature = GetMethodSignature(method);

                var entityName = this.Type.Name.Substring(0, this.Type.Name.Length - "Controller".Length);
                var genericArgString = method.GetReturnType().ToTypeScript()
                            .ToString(p => p.Namespace != this.Type.Namespace, true, false);

                var beautifulParamList = parameters.ToSmartEnumerable()
                            .Select(p => $"{p.Value.Name}: {p.Value.Name}{(p.IsLast ? "" : ",")}");

                bool isGetOrDelete = httpVerb == HttpMethodAttribute.Get || httpVerb == HttpMethodAttribute.Delete;

                IEnumerable<string> paramsBody;
                if (isGetOrDelete)
                {
                    paramsBody =
                        new[] { $@"(config || (config = {{}})).params = {{" }
                        .Concat(beautifulParamList)
                        .Concat(new[] {$"}};",
                        $"return restApi.{httpVerb}<{genericArgString}>('/{entityName}/{method.Name}', config);"
                    });
                }
                else
                {
                    paramsBody =
                        new[] { $@"let data = {{" }
                        .Concat(beautifulParamList)
                        .Concat(new[] {$"}};",
                        $"return restApi.{httpVerb}<{genericArgString}>('/{entityName}/{method.Name}', data, config);"
                    });
                }

                return new[] { $"public {signature} {{", paramsBody.Join(Environment.NewLine), $"}}" };
            }).JoinNewLine();
            return new[] {
                $"export class {ServiceName} {{",
                body,
                $"}}"
            };
        }

        /// <summary>
        /// Return method signature in typescript of a C# method
        /// </summary>
        /// <param name="method">The method</param>
        public string GetMethodSignature(MethodInfo method)
        {
            var options = new ToTypeScriptOptions(type => type.Namespace != this.Type.Namespace, true, this.StrictNullCheck);
            var arguments = method.GetParameters()
                    .Select(param => $", {param.Name}: {param.ParameterType.ToTypeScript().ToString(options)}")
                    .Join("");

            return $"{method.Name}(restApi: RestApi{arguments}, config?: RequestConfig)";
        }
    }
}
