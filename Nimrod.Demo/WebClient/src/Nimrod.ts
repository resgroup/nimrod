import * as angular from 'angular';

export interface RequestConfig extends angular.IRequestShortcutConfig {
}

export interface RestApi {
    Delete<T>(url: string, config?: RequestConfig): angular.IHttpPromise<T>;
    Get<T>(url: string, config?: RequestConfig): angular.IHttpPromise<T>;
    Post<T>(url: string, data: any, config?: RequestConfig): angular.IHttpPromise<T>;
    Put<T>(url: string, data: any, config?: RequestConfig): angular.IHttpPromise<T>;
}
/**
 * Service that handle HTTP CRUD request to the server
 * Performs HTTP GET, POST, PUT, DELETE and returns promise
 * @param {!angular.$http} $http The Angular http service.
 * @param {!angular.$log} $log The Angular log service.
 * @constructor
 */
export class RestApiImpl implements RestApi {
    static $inject = [
        '$http',
    ];
    constructor(
        private $http: angular.IHttpService,
    ) { }

    public Delete<T>(url: string, config?: angular.IRequestShortcutConfig) {
        return this.$http.delete<T>(url, config);
    }

    public Get<T>(url: string, config?: angular.IRequestShortcutConfig) {
        return this.$http.get<T>(url, config);
    }

    public Post<T>(url: string, data: any, config?: angular.IRequestShortcutConfig) {
        return this.$http.post<T>(url, data, config);
    }

    public Put<T>(url: string, data: any, config?: angular.IRequestShortcutConfig) {
        return this.$http.put<T>(url, data, config);
    }
}
