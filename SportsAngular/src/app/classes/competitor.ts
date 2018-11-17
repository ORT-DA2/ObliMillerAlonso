import { Sport } from "./sport";

export class Competitor {
    id:number;
    name:string;
    picture:string;
    sport:Sport;


    constructor(name:string, picture:string,sport:Sport) {
        this.name = name;
        this.picture = picture;
        this.sport = sport;
    }
}