import { Competitor } from "./competitor";
import {User} from "./user";

export class Favorite {
    id:number;
    user:User;
    competitor:Competitor;
   
    constructor(id:number, user:User, competitor:Competitor) {
        this.id = id;
        this.user = user;
        this.competitor = competitor;
    }
}