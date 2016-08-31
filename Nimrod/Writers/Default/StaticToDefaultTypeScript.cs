using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Nimrod.Writers.Default
{
    public class StaticToDefaultTypeScript : StaticToTypeScript
    {
        public override IEnumerable<string> GetRestApiLines() => new[] {
            $@"namespace Nimrod {{
                export interface IRestApi {{
                Delete<T>(url: string, config?: IRequestConfig): IPromise<T>;
                Get<T>(url: string, config?: IRequestConfig): IPromise<T>;
                Post<T>(url: string, data: any, config?: IRequestConfig): IPromise<T>;
                Put<T>(url: string, data: any, config?: IRequestConfig): IPromise<T>;
              }}
            }}"
        };


        public override IEnumerable<string> GetPromiseLines() => new[] {
            $@"namespace Nimrod {{
                export interface IPromise<T> {{
                    then<U>(onFulfilled ?: (value: T) => void, onRejected?: (error: any) => void): IPromise<U>;
                    catch< U > (onRejected ?: (error: any) => void): IPromise<U>;
                }}
            }}"
        };
    }
}
