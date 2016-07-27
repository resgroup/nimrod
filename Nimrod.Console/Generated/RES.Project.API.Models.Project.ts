namespace RES.Project.API.Models {
    export interface IProject {
        Id: number;
        Key: string;
        Name: string;
        Phase: RES.Project.API.Models.IProjectPhase;
        Latitude: number;
        Longitude: number;
        ProjectType: string;
        ProjectTypeDescription: string;
        Region: string;
        Market: string;
        Status: string;
        DateCreated: Date;
        Country: string;
    }
}
