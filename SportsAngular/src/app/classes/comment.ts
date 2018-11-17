import { User } from "./user";
import { Match } from "./match";

export class Comment {
    id:number;
    user:User;
    match:Match;
    text:string;

    constructor(user:User, text:string) {
        this.user = user;
        this.text = text;
    }
}