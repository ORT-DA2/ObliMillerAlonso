
export class CompetitorScoreDTO {
    competitorId:number;
    score:number;


    constructor(competitorId:number, score:number) {
        this.competitorId = competitorId;
        this.score = score;
    }
}