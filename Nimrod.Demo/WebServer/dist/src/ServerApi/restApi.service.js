var Nimrod;
(function (Nimrod) {
    var Demo;
    (function (Demo) {
        var RestApi;
        (function (RestApi_1) {
            "use strict";
            var RestApi = (function () {
                function RestApi($http) {
                    this.$http = $http;
                }
                RestApi.prototype.Delete = function (url, config) {
                    return this.$http.delete(url, config);
                };
                RestApi.prototype.Get = function (url, config) {
                    return this.$http.get(url, config).then(function (datas) { return datas.data; });
                };
                RestApi.prototype.Post = function (url, data, config) {
                    return this.$http.post(url, data, config);
                };
                RestApi.prototype.Put = function (url, data, config) {
                    return this.$http.put(url, data, config);
                };
                RestApi.$inject = [
                    '$http'
                ];
                return RestApi;
            }());
            Demo.service("restApi", RestApi);
        })(RestApi = Demo.RestApi || (Demo.RestApi = {}));
    })(Demo = Nimrod.Demo || (Nimrod.Demo = {}));
})(Nimrod || (Nimrod = {}));
//# sourceMappingURL=restApi.service.js.map