import { User } from "./user";

export class Comment {
    id:number;
    text:string;
    date:String;
    user:User;

    constructor(id:number, text:string, date:string,user:User) {
        this.id = id;
        this.text = text;
        this.date = date;
        this.user = user;
    }
}


