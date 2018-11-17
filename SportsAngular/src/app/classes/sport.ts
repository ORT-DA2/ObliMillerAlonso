import { Competitor } from "./competitor";
import { Match } from "./match";

export class Sport {
    id:number;
    name:string;
    amount:number;
    competitors:Array<Competitor>
    matches:Array<Match>

    constructor( name:string, amount:number) {
        this.name = name;
        this.amount = amount;
    }
}