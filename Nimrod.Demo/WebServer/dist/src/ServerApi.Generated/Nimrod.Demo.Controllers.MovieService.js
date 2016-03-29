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
//# sourceMappingURL=Nimrod.Demo.Controllers.MovieService.js.map