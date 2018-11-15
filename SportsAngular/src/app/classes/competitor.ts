import { Sport } from "./sport";

export class Competitor {
    id:number;
    name:string;
    picture:string;
    sport:Sport;


    constructor(id:number, name:string, picture:string,sport:Sport) {
        this.id = id;
        this.name = name;
        this.picture = picture;
        this.sport = sport;
    }
}