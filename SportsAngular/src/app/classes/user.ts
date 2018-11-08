
export class User {
    id:number;
    firstName:string;
    lastName:string;
    email:string;
    password:string;
    userName:string;
    isAdmin:boolean;

    constructor(id:number, firstName:string, lastName:string, email:string, password:string, userName:string, isAdmin:boolean) {
        this.id = id;
        this.firstName = firstName;
        this.lastName = lastName;
        this.email = email;
        this.password = password;
        this.userName = userName;
        this.isAdmin = isAdmin;
    }
}