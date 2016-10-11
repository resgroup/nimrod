import { RestApi, RequestConfig } from '../Nimrod';
import * as Nimrod_Demo_Models from './Nimrod.Demo.Models';
export class MovieService {
    public Movie(restApi: RestApi, guid: string, config?: RequestConfig) {
        (config || (config = {})).params = {
            guid: guid
        };
        return restApi.Get<Nimrod_Demo_Models.Movie>('/Movie/Movie', config);
    }
    public Movies(restApi: RestApi, config?: RequestConfig) {
        (config || (config = {})).params = {
            
        };
        return restApi.Get<Nimrod_Demo_Models.Movie[]>('/Movie/Movies', config);
    }
    public Add(restApi: RestApi, movie: Nimrod_Demo_Models.Movie, config?: RequestConfig) {
        let data = {
            movie: movie
        };
        return restApi.Post<Nimrod_Demo_Models.Movie>('/Movie/Add', data, config);
    }
    public Delete(restApi: RestApi, guid: string, config?: RequestConfig) {
        let data = {
            guid: guid
        };
        return restApi.Post<boolean>('/Movie/Delete', data, config);
    }
}
