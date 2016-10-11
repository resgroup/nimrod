import { Movie } from '../Nimrod.Generated/Nimrod.Demo.Models';
import { MovieService } from '../Nimrod.Generated/Nimrod.Demo.Controllers';
import { RestApi } from '../Nimrod';
import { IComponentOptions } from 'angular';

class MoviesController {

    movies: Movie[];

    static $inject = [
        'restApi',
        'movieService'
    ];
    constructor(
        private restApi: RestApi,
        private moviesApi: MovieService
    ) {
        this.loadMovies();
    }

    loadMovies() {
        this.moviesApi.Movies(this.restApi).then(response => {
            this.movies = response.data;
        });
    }

    addMovie() {
        let newMovie: Movie = {
            Name: 'The God Father',
            Guid: null,
            Actors: [],
            Rating: 8
        };
        this.moviesApi.Add(this.restApi, newMovie).then(response => {
            this.loadMovies();
        });
    }
    deleteMovie(movie: Movie) {
        this.moviesApi.Delete(this.restApi, movie.Guid).then(response => {
            this.loadMovies();
        });
    }

    range(n: number) {
        return new Array(n);
    };

}

const options: IComponentOptions = {
    controller: MoviesController,
    template: `
            <div ng-repeat='movie in $ctrl.movies'>
                <h1>{{movie.Name}}
                    <ul class='star-rating'> 
                        <li ng-repeat='star in $ctrl.range(movie.Rating) track by $index' 
                            class='star'> 
                            <i class='glyphicon glyphicon-star'></i> 
                        </li> 
                    </ul>
                </h1>
                <div ng-if='movie.Actors.length > 0'>
                    <h2>Actors</h2>
                    <ul>
                        <li ng-repeat='actor in movie.Actors'>{{actor}}</li>
                    </ul>
                </div>
                <button ng-click='$ctrl.deleteMovie(movie)'>Delete</button>    
            </div>
            <button ng-click='$ctrl.addMovie()'>Add One</button>
        `
};

export default options;
