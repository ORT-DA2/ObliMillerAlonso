import { User } from "./user";
import { Match } from "./match";

export class Comment {
    id:number;
    user:User;
    match:Match;
    date:String;
    text:String;

    constructor(text:string) {
        this.text = text;
    }
}
