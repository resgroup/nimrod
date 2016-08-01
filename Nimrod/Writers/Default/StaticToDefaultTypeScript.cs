using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Nimrod.Writers.Default
{
    public class StaticToDefaultTypeScript : StaticToTypeScript
    {
        public override IEnumerable<string> GetRestApiLines()
        {
            yield return "namespace Nimrod {";
            yield return "export interface IRestApi {";
            yield return "    Delete<T>(url: string, config?: IRequestConfig): IPromise<T>;";
            yield return "    Get<T>(url: string, config?: IRequestConfig): IPromise<T>;";
            yield return "    Post<T>(url: string, data: any, config?: IRequestConfig): IPromise<T>;";
            yield return "    Put<T>(url: string, data: any, config?: IRequestConfig): IPromise<T>;";
            yield return "}}";
        }

        public override IEnumerable<string> GetPromiseLines()
        {
            yield return "namespace Nimrod {";
            yield return "export interface IPromise<U> {";
            yield return "    then<U>(onFulfilled ?: (value: T) => void, onRejected?: (error: any) => void): IPromise<U>;";
            yield return "    catch< U > (onRejected ?: (error: any) => void): IPromise<U>;";
            yield return "}}";
        }
    }
}
