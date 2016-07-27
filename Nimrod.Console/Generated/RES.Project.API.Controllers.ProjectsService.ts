namespace RES.Project.API.Controllers {
    export interface IProjectsService {
        Get(restApi: Nimrod.IRestApi, config?: Nimrod.IRequestShortcutConfig): Nimrod.IPromise<RES.Project.API.Models.IProject>;
    }
    export class ProjectsService implements IProjectsService {
        public Get(restApi: Nimrod.IRestApi, config?: Nimrod.IRequestShortcutConfig): Nimrod.IPromise<RES.Project.API.Models.IProject> {
            (config || (config = {})).params = {
            };
            return restApi.Get<RES.Project.API.Models.IProject>('/Projects/Get', config);
        }
    }
    service('serverApi.ProjectsService', ProjectsService);
}
