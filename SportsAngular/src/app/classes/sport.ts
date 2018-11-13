import { Competitor } from "./competitor";

export class Sport {
    id:number;
    name:string;
    amount:number;
    competitors:Array<Competitor>

    constructor(id:number, name:string, amount:number) {
        this.id = id;
        this.name = name;
        this.amount = amount;
    }
}