module Nimrod.Demo {
    "use strict";

    // Create the module and define its dependencies.
    angular.module("nimrod.demo", []);


    function ngModule(): ng.IModule {
        return angular.module("nimrod.demo");
    }

    export function controller(camelCaseName: string, controllerConstructor: Function) {
        ngModule().controller(camelCaseName, controllerConstructor);
    }

    export function directive(camelCaseName: string, directive: ng.IDirectiveFactory) {
        ngModule().directive(camelCaseName, directive);
    }

    export function service(name: string, serviceConstructor: Function) {
        ngModule().service(name, serviceConstructor);
    }

    export function filter(name: string, filterConstructor: Function) {
        ngModule().filter(name, filterConstructor);
    }
    export function component(name: string, config: ng.IComponentOptions) {
        ngModule().component(name, config);
    }
}
