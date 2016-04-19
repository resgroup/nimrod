var Nimrod;
(function (Nimrod) {
    var Demo;
    (function (Demo) {
        // Create the module and define its dependencies.
        angular.module('nimrod.demo', []);
        function ngModule() {
            return angular.module('nimrod.demo');
        }
        function controller(camelCaseName, controllerConstructor) {
            ngModule().controller(camelCaseName, controllerConstructor);
        }
        Demo.controller = controller;
        function directive(camelCaseName, directive) {
            ngModule().directive(camelCaseName, directive);
        }
        Demo.directive = directive;
        function service(name, serviceConstructor) {
            ngModule().service(name, serviceConstructor);
        }
        Demo.service = service;
        function filter(name, filterConstructor) {
            ngModule().filter(name, filterConstructor);
        }
        Demo.filter = filter;
        function component(name, config) {
            ngModule().component(name, config);
        }
        Demo.component = component;
    })(Demo = Nimrod.Demo || (Nimrod.Demo = {}));
})(Nimrod || (Nimrod = {}));
//# sourceMappingURL=nimrod.demo.module.js.map