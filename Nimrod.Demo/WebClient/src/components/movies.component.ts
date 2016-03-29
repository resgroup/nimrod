module Nimrod.Demo {

    class MoviesController {

        movies: Models.IMovie[];

        static $inject = [
            'restApi',
            'serverApi.MovieService'
        ]
        constructor(
            private restApi: IRestApi,
            private moviesApi: Controllers.IMovieService
        ) {
            this.loadMovies();
        }

        loadMovies() {
            this.moviesApi.Movies(this.restApi).then(response => {
                this.movies = response;
            });
        }

        addMovie() {
            var newMovie: Models.IMovie = {
                Name: "The God Father",
                Id: null,
                Actors: [],
                Rating: 8
            };
            this.moviesApi.Add(this.restApi, newMovie).then(response => {
                this.loadMovies();
            });
        }
        deleteMovie(id: number) {
            this.moviesApi.Delete(this.restApi, id).then(response => {
                this.loadMovies();
            });
        }

        range(n: number) {
            return new Array(n);
        };

    }

    component("ndMovies", {
        controller: MoviesController,
        bindings: {
        },
        template: `
            <div ng-repeat="movie in $ctrl.movies">
                <h1>{{movie.Name}}
                    <ul class="star-rating"> 
                        <li ng-repeat="star in $ctrl.range(movie.Rating) track by $index" 
                            class="star"> 
                            <i class="glyphicon glyphicon-star"></i> 
                        </li> 
                    </ul>
                </h1>
                <div ng-if="movie.Actors.length > 0">
                    <h2>Actors</h2>
                    <ul>
                        <li ng-repeat="actor in movie.Actors">{{actor}}</li>
                    </ul>
                </div>
                <button ng-click="$ctrl.deleteMovie(movie.Id)">Delete</button>    
            </div>
            <button ng-click="$ctrl.addMovie()">Add One</button>
        `
    });
}