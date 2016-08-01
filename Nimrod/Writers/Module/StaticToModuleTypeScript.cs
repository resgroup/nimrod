using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Nimrod.Writers.Module
{
    public class StaticToModuleTypeScript : StaticToTypeScript
    {
        public override IEnumerable<string> GetRestApiLines()
        {
            yield return "import IRequestConfig from '../Nimrod/IRequestConfig'";
            yield return "import IPromise from './IPromise'";
            yield return "interface IRestApi {";
            yield return "    Delete<T>(url: string, config?: IRequestConfig): IPromise<T>;";
            yield return "    Get<T>(url: string, config?: IRequestConfig): IPromise<T>;";
            yield return "    Post<T>(url: string, data: any, config?: IRequestConfig): IPromise<T>;";
            yield return "    Put<T>(url: string, data: any, config?: IRequestConfig): IPromise<T>;";
            yield return "}";
            yield return "export default IRestApi;";
        }

        public override IEnumerable<string> GetPromiseLines()
        {
            yield return "interface IPromise<T> {";
            yield return "    then<U>(onFulfilled ?: (value: T) => void, onRejected?: (error: any) => void): IPromise<U>;";
            yield return "    catch< U > (onRejected ?: (error: any) => void): IPromise<U>;";
            yield return "}";
            yield return "export default IPromise;";
        }
    }
}
