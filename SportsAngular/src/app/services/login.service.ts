import { Injectable } from "@angular/core";
import {
  Http,
  Headers,
  Response,
  RequestOptions,
  RequestMethod
} from "@angular/http";
import { Observable, throwError } from "rxjs";
import { map, tap, catchError } from "rxjs/operators";
import { environment } from '../../environments/environment';

import { User } from "../classes/user";

@Injectable({
  providedIn: 'root'
})
export class LoginService {

  public token: string;
  public loggedUser: User;

  constructor(
    private _httpService: Http
    ) { }

  login(username: string, password: string) {
    let headers = new Headers({ "Content-Type": "application/json" });
    let options = new RequestOptions({ headers: headers });
    return this._httpService
      .post(
        environment.apiUrl + "users/login", { "Username": username, "Password": password },
        options
      )
      .pipe(
        map((response: Response) => {
          let tokenModel = response.json();
          if (tokenModel) {
            localStorage.setItem("user_token", tokenModel.token);
          }
          return tokenModel;
        }),
        catchError(this.handleError)
      );
  }

  isAdminUser() {
    let myToken = localStorage.getItem("user_token");
    let headers = new Headers({
      "Content-Type": "application/json",
      "token": myToken
    });
    let options = new RequestOptions({ headers: headers });
    let address = environment.apiUrl + "users/current";
    return this._httpService
      .get(address, options)
      .pipe(
        map((response: Response) => {
          let user = <User>response.json();
          let admin = user.isAdmin.toString();
          localStorage.setItem("isAdmin", admin);
          localStorage.setItem("currentUserId", user.id.toString());
        }),
        catchError(this.handleError)
      )

  }


  logout() {
    let headers = new Headers({
      "Content-Type": "application/json",
      "token": localStorage.getItem("user_token")
    });
    let options = new RequestOptions({ headers: headers });
    return this._httpService
      .delete(environment.apiUrl + "users/logout", options)
      .pipe(map((response: Response) => {
        localStorage.removeItem('user_token');
        localStorage.removeItem('isAdmin');
        localStorage.removeItem('currentUserId');

      }), catchError(this.handleError));
  }

  private handleError(error: Response) {
    if (error.status == 0)
      return throwError(new Error("Ocurrió un error inesperado en el servidor."));
    let body = error["_body"];
    return throwError(new Error(body));
  }
}
