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
        public string ServiceName => this.Type.Name.Replace("Controller", "Service");
        public virtual bool NeedNameSpace => false;

        public ControllerToTypeScript(TypeScriptType type, bool strictNullCheck, bool singleFile)
            : base(type, strictNullCheck, singleFile)
        {
            if (!type.Type.IsWebController())
            {
                throw new ArgumentOutOfRangeException($"Type {type.Name} MUST extends System.Web.Mvc.Controller or System.Web.Http.IHttpControler", nameof(type));
            }
        }
        public override IEnumerable<string> GetImports()
        {
            var actions = TypeDiscovery.GetWebControllerActions(this.Type.Type);
            var importedTypes = actions.SelectMany(action => action.GetReturnTypeAndParameterTypes())
                                       .Distinct();

            var imports = ModuleHelper.GetTypesToImport(importedTypes)
                                 .Where(type => this.SingleFile ? true : type.Namespace != this.Type.Namespace)
                                 .Select(t => ModuleHelper.GetImportLine(t, this.SingleFile));
            return imports.Concat(new[] {
                $"import {{ RestApi }} from '../Nimrod';",
                $"import {{ Promise }} from '../Nimrod';",
                $"import {{ RequestConfig }} from '../Nimrod';"
            });
        }

        public override IEnumerable<string> GetLines() => GetImplementation().Concat(this.SingleFile ? $"export default { this.Type.Name};" : "");

        private IEnumerable<string> GetImplementation()
        {
            var body = TypeDiscovery.GetWebControllerActions(this.Type.Type).SelectMany(method =>
            {
                var httpVerb = method.FirstOrDefaultHttpMethodAttribute();
                var parameters = method.GetParameters();

                var signature = GetMethodSignature(method);

                var entityName = this.Type.Name.Substring(0, this.Type.Name.Length - "Controller".Length);
                var genericArgString = method.GetReturnType().ToTypeScript().ToString(NeedNameSpace, true, false);

                var beautifulParamList = parameters
                            .Select(p => $"{p.Name}: {p.Name}")
                            .Join($",{Environment.NewLine}");



                bool isGetOrDelete = httpVerb == HttpMethodAttribute.Get || httpVerb == HttpMethodAttribute.Delete;

                var paramsBody = isGetOrDelete ?
                    $@"(config || (config = {{}})).params = {{
                    {beautifulParamList}
                    }};
                    return restApi.{httpVerb}<{genericArgString}>('/{entityName}/{method.Name}', config);" :

                   $@"let data = {{
                    {beautifulParamList}
                    }};
                    return restApi.{httpVerb}<{genericArgString}>('/{entityName}/{method.Name}', data, config);";
                return new[] {
                    $"public {signature} {{",
                    paramsBody,
                    $"}}"
                };
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
            var options = new ToTypeScriptOptions(NeedNameSpace, true, this.StrictNullCheck);
            var arguments = method.GetParameters()
                    .Select(param => $", {param.Name}: {param.ParameterType.ToTypeScript().ToString(options)}")
                    .Join("");
            var returnType = method.GetReturnType().ToTypeScript().ToString(NeedNameSpace, true, StrictNullCheck);

            return $"{method.Name}(restApi: RestApi{arguments}, config?: RequestConfig): Promise<{returnType}>";
        }
    }
}
