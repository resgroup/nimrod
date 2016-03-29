module Nimrod.Demo.Controllers {
    export interface IMovieService {
        Movie(restApi: Nimrod.IRestApi, id: number, config?: Nimrod.IRequestShortcutConfig): Nimrod.IPromise<Nimrod.Demo.Models.IMovie>;
        Movies(restApi: Nimrod.IRestApi, config?: Nimrod.IRequestShortcutConfig): Nimrod.IPromise<Nimrod.Demo.Models.IMovie[]>;
        Add(restApi: Nimrod.IRestApi, movie: Nimrod.Demo.Models.IMovie, config?: Nimrod.IRequestShortcutConfig): Nimrod.IPromise<Nimrod.Demo.Models.IMovie>;
        Delete(restApi: Nimrod.IRestApi, id: number, config?: Nimrod.IRequestShortcutConfig): Nimrod.IPromise<boolean>;
    }
    export class MovieService implements IMovieService {
        public Movie(restApi: Nimrod.IRestApi, id: number, config?: Nimrod.IRequestShortcutConfig): Nimrod.IPromise<Nimrod.Demo.Models.IMovie> {
            (config || (config = {})).params = {
                id: id,
            };
            return restApi.Get<Nimrod.Demo.Models.IMovie>('/Movie/Movie', config);
        }
        public Movies(restApi: Nimrod.IRestApi, config?: Nimrod.IRequestShortcutConfig): Nimrod.IPromise<Nimrod.Demo.Models.IMovie[]> {
            (config || (config = {})).params = {
            };
            return restApi.Get<Nimrod.Demo.Models.IMovie[]>('/Movie/Movies', config);
        }
        public Add(restApi: Nimrod.IRestApi, movie: Nimrod.Demo.Models.IMovie, config?: Nimrod.IRequestShortcutConfig): Nimrod.IPromise<Nimrod.Demo.Models.IMovie> {
            var data = {
                movie: movie,
            };
            return restApi.Post<Nimrod.Demo.Models.IMovie>('/Movie/Add', data, config);
        }
        public Delete(restApi: Nimrod.IRestApi, id: number, config?: Nimrod.IRequestShortcutConfig): Nimrod.IPromise<boolean> {
            var data = {
                id: id,
            };
            return restApi.Post<boolean>('/Movie/Delete', data, config);
        }
    }
    service('serverApi.MovieService', MovieService);
}
