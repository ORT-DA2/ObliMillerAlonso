import { CompetitorScore } from "./competitorScore";
import { Sport } from "./sport";
import { Comment } from "./comment";

export class Match {
    id:number;
    date:Date;
    competitorScores:Array<CompetitorScore>;
    sport:Sport;
    comments:Array<Comment>;

    constructor(date:Date, competitorScores:Array<CompetitorScore>, sport:Sport) {
        this.date = date;
        this.competitorScores = competitorScores;
        this.sport = sport;
    }
}