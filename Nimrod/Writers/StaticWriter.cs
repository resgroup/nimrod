using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nimrod
{
    public class StaticWriter
    {
        public string Write(ModuleType module)
        {
            string content;
            if (module == ModuleType.TypeScript)
            {
                content = @"
module Nimrod {
    export interface IRestApi {
        Delete<T>(url: string, config?: IRequestShortcutConfig): IPromise<T>;
        Get<T>(url: string, config?: IRequestShortcutConfig): IPromise<T>;
        Post<T>(url: string, data: any, config?: IRequestShortcutConfig): IPromise<T>;
        Put<T>(url: string, data: any, config?: IRequestShortcutConfig): IPromise<T>;
    }
}
";
            }
            else
            {
                content = @"
import Nimrod = require('../Nimrod/Nimrod');
interface IRestApi {
    Delete<T>(url: string, config?: Nimrod.IRequestShortcutConfig): Nimrod.IPromise<T>;
    Get<T>(url: string, config?: Nimrod.IRequestShortcutConfig): Nimrod.IPromise<T>;
    Post<T>(url: string, data: any, config?: Nimrod.IRequestShortcutConfig): Nimrod.IPromise<T>;
    Put<T>(url: string, data: any, config?: Nimrod.IRequestShortcutConfig): Nimrod.IPromise<T>;
}
export = IRestApi;
";
            }
            return content;
        }
    }
}
