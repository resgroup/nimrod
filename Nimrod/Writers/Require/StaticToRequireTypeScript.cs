using System;
using System.Collections.Generic;
using System.Linq;

namespace Nimrod.Writers.Require
{
    public class StaticToRequireTypeScript : StaticToTypeScript
    {
        public override IEnumerable<string> GetRestApiLines() => new[]
        {
            $@"import Nimrod = require('../Nimrod/Nimrod');
            interface IRestApi {{
                Delete<T>(url: string, config ?: Nimrod.IRequestConfig): Nimrod.IPromise<T>;
                Get<T>(url: string, config?: Nimrod.IRequestConfig): Nimrod.IPromise<T>;
                Post<T>(url: string, data: any, config?: Nimrod.IRequestConfig): Nimrod.IPromise<T>;
                Put<T>(url: string, data: any, config?: Nimrod.IRequestConfig): Nimrod.IPromise<T>;
                }}
            export = IRestApi;"
        };

        public override IEnumerable<string> GetPromiseLines() => new[]
        {
            $@"interface IPromise<T> {{
                then<U>(onFulfilled ?: (value: T) => void, onRejected?: (error: any) => void): IPromise<U>;
                catch< U > (onRejected ?: (error: any) => void): IPromise<U>;
            }}
            export = IRestApi;"
        };
    }
}
