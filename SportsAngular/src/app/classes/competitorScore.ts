import { Competitor } from "./competitor";

export class CompetitorScore {
    id:number;
    competitor:Competitor;
    score:number;


    constructor(competitor:Competitor, score:number) {
        this.competitor = competitor;
        this.score = score;
    }
}