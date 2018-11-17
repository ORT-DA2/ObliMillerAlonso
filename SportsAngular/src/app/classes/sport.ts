import { Competitor } from "./competitor";

export class Sport {
    id:number;
    name:string;
    amount:number;
    competitors:Array<Competitor>

    constructor( name:string, amount:number) {
        this.name = name;
        this.amount = amount;
    }
}