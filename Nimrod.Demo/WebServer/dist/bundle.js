var Nimrod;
(function (Nimrod) {
    var Demo;
    (function (Demo) {
        "use strict";
        angular.module("nimrod.demo", []);
        function ngModule() {
            return angular.module("nimrod.demo");
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
var Nimrod;
(function (Nimrod) {
    var Demo;
    (function (Demo) {
        var Controllers;
        (function (Controllers) {
            var MovieService = (function () {
                function MovieService() {
                }
                MovieService.prototype.Movie = function (restApi, id, config) {
                    (config || (config = {})).params = {
                        id: id,
                    };
                    return restApi.Get('/Movie/Movie', config);
                };
                MovieService.prototype.Movies = function (restApi, config) {
                    (config || (config = {})).params = {};
                    return restApi.Get('/Movie/Movies', config);
                };
                MovieService.prototype.Add = function (restApi, movie, config) {
                    var data = {
                        movie: movie,
                    };
                    return restApi.Post('/Movie/Add', data, config);
                };
                MovieService.prototype.Delete = function (restApi, id, config) {
                    var data = {
                        id: id,
                    };
                    return restApi.Post('/Movie/Delete', data, config);
                };
                return MovieService;
            }());
            Controllers.MovieService = MovieService;
            Demo.service('serverApi.MovieService', MovieService);
        })(Controllers = Demo.Controllers || (Demo.Controllers = {}));
    })(Demo = Nimrod.Demo || (Nimrod.Demo = {}));
})(Nimrod || (Nimrod = {}));
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
var Nimrod;
(function (Nimrod) {
    var Demo;
    (function (Demo) {
        var MoviesController = (function () {
            function MoviesController(restApi, moviesApi) {
                this.restApi = restApi;
                this.moviesApi = moviesApi;
                this.loadMovies();
            }
            MoviesController.prototype.loadMovies = function () {
                var _this = this;
                this.moviesApi.Movies(this.restApi).then(function (response) {
                    _this.movies = response;
                });
            };
            MoviesController.prototype.addMovie = function () {
                var _this = this;
                var newMovie = {
                    Name: "The God Father",
                    Id: null,
                    Actors: [],
                    Rating: 8
                };
                this.moviesApi.Add(this.restApi, newMovie).then(function (response) {
                    _this.loadMovies();
                });
            };
            MoviesController.prototype.deleteMovie = function (id) {
                var _this = this;
                this.moviesApi.Delete(this.restApi, id).then(function (response) {
                    _this.loadMovies();
                });
            };
            MoviesController.prototype.range = function (n) {
                return new Array(n);
            };
            ;
            MoviesController.$inject = [
                'restApi',
                'serverApi.MovieService'
            ];
            return MoviesController;
        }());
        Demo.component("ndMovies", {
            controller: MoviesController,
            bindings: {},
            template: "\n            <div ng-repeat=\"movie in $ctrl.movies\">\n                <h1>{{movie.Name}}\n                    <ul class=\"star-rating\"> \n                        <li ng-repeat=\"star in $ctrl.range(movie.Rating) track by $index\" \n                            class=\"star\"> \n                            <i class=\"glyphicon glyphicon-star\"></i> \n                        </li> \n                    </ul>\n                </h1>\n                <div ng-if=\"movie.Actors.length > 0\">\n                    <h2>Actors</h2>\n                    <ul>\n                        <li ng-repeat=\"actor in movie.Actors\">{{actor}}</li>\n                    </ul>\n                </div>\n                <button ng-click=\"$ctrl.deleteMovie(movie.Id)\">Delete</button>    \n            </div>\n            <button ng-click=\"$ctrl.addMovie()\">Add One</button>\n        "
        });
    })(Demo = Nimrod.Demo || (Nimrod.Demo = {}));
})(Nimrod || (Nimrod = {}));
//# sourceMappingURL=bundle.js.map