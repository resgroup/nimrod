using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Nimrod.Writers.Require
{
    public class StaticToRequireTypeScript : StaticToTypeScript
    {
        public override IEnumerable<string> GetRestApiLines()
        {
            yield return "import Nimrod = require('../Nimrod/Nimrod');";
            yield return "interface IRestApi {";
            yield return "    Delete<T>(url: string, config?: Nimrod.IRequestConfig): Nimrod.IPromise<T>;";
            yield return "    Get<T>(url: string, config?: Nimrod.IRequestConfig): Nimrod.IPromise<T>;";
            yield return "    Post<T>(url: string, data: any, config?: Nimrod.IRequestConfig): Nimrod.IPromise<T>;";
            yield return "    Put<T>(url: string, data: any, config?: Nimrod.IRequestConfig): Nimrod.IPromise<T>;";
            yield return "    }";
            yield return "export = IRestApi;";
        }

        public override IEnumerable<string> GetPromiseLines()
        {
            yield return "interface IPromise<T> {";
            yield return "    then<U>(onFulfilled ?: (value: T) => void, onRejected?: (error: any) => void): IPromise<U>;";
            yield return "    catch< U > (onRejected ?: (error: any) => void): IPromise<U>;";
            yield return "}";
            yield return "export = IRestApi;";
        }
    }
}
