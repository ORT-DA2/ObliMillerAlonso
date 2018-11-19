import { Sport } from "./sport";

export class Fixture {
    id:number;
    text:string;

    constructor(id:number, text:string) {
        this.id = id;
        this.text = text;
    }
}
