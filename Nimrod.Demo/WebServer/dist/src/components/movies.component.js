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
                    Name: 'The God Father',
                    Guid: null,
                    Actors: [],
                    Rating: 8
                };
                this.moviesApi.Add(this.restApi, newMovie).then(function (response) {
                    _this.loadMovies();
                });
            };
            MoviesController.prototype.deleteMovie = function (movie) {
                var _this = this;
                this.moviesApi.Delete(this.restApi, movie.Guid).then(function (response) {
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
        Demo.component('ndMovies', {
            controller: MoviesController,
            bindings: {},
            template: "\n            <div ng-repeat='movie in $ctrl.movies'>\n                <h1>{{movie.Name}}\n                    <ul class='star-rating'> \n                        <li ng-repeat='star in $ctrl.range(movie.Rating) track by $index' \n                            class='star'> \n                            <i class='glyphicon glyphicon-star'></i> \n                        </li> \n                    </ul>\n                </h1>\n                <div ng-if='movie.Actors.length > 0'>\n                    <h2>Actors</h2>\n                    <ul>\n                        <li ng-repeat='actor in movie.Actors'>{{actor}}</li>\n                    </ul>\n                </div>\n                <button ng-click='$ctrl.deleteMovie(movie)'>Delete</button>    \n            </div>\n            <button ng-click='$ctrl.addMovie()'>Add One</button>\n        "
        });
    })(Demo = Nimrod.Demo || (Nimrod.Demo = {}));
})(Nimrod || (Nimrod = {}));
//# sourceMappingURL=movies.component.js.map