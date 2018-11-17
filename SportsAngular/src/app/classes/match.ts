import { CompetitorScore } from "./competitorScore";
import { Sport } from "./sport";
import { Comment } from "./comment";

export class Match {
    id:number;
    date:string;
    competitors:Array<CompetitorScore>;
    sport:Sport;
    comments:Array<Comment>;

    constructor(date:string, competitors:Array<CompetitorScore>, sport:Sport) {
        this.date = date;
        this.competitors = competitors;
        this.sport = sport;
    }
}