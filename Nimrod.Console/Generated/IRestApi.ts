
namespace Nimrod {
    export interface IRestApi {
        Delete<T>(url: string, config?: IRequestShortcutConfig): IPromise<T>;
        Get<T>(url: string, config?: IRequestShortcutConfig): IPromise<T>;
        Post<T>(url: string, data: any, config?: IRequestShortcutConfig): IPromise<T>;
        Put<T>(url: string, data: any, config?: IRequestShortcutConfig): IPromise<T>;
    }
}
