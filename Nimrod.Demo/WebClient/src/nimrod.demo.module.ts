import { module } from 'angular';
import { RestApiImpl } from './Nimrod';
import { MovieService } from './Nimrod.Generated/Nimrod.Demo.Controllers';
import MoviesComponent from './components/movies.component';
import * as boot from 'bootstrap';


export default module('nimrod.demo', [])
    .service('restApi', RestApiImpl)
    .service('movieService', MovieService)
    .component('ndMovies', MoviesComponent);
