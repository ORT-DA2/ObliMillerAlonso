import { User } from "./user";
import { Match } from "./match";

export class Comment {
    id:number;
    user:User;
    match:Match;
    date:String;
    text:string;

    constructor(user:User, text:string,date:string) {
        this.user = user;
        this.text = text;
        this.date = date;
    }
}
