module Nimrod.Demo.RestApi {
    "use strict";

    class RestApi implements Nimrod.IRestApi {
        static $inject = [
            '$http'
        ];
        constructor(
            private $http: ng.IHttpService
        ) {
        }

        public Delete<T>(url: string, config?: ng.IRequestShortcutConfig) {
            return this.$http.delete<T>(url, config);
        }

        public Get<T>(url: string, config?: ng.IRequestShortcutConfig) {
            return this.$http.get<T>(url, config).then(datas => datas.data);
        }

        public Post<T>(url: string, data: any, config?: ng.IRequestShortcutConfig) {
            return this.$http.post<T>(url, data, config);
        }

        public Put<T>(url: string, data: any, config?: ng.IRequestShortcutConfig) {
            return this.$http.put<T>(url, data, config);
        }
    }

    service("restApi", RestApi);
}
