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
import { AlertService } from "./alert.service";

@Injectable({
  providedIn: 'root'
})

export class UserService {

  constructor(
    private _httpService: Http,
    private alertService: AlertService
  ) { }

  create(data: any) {
    let headers = new Headers({
      "Content-Type": "application/json",
      Token: localStorage.getItem("user_token")
    });
    let options = new RequestOptions({ headers: headers });
    return this._httpService
      .post(
        environment.apiUrl +
        "users", JSON.stringify(data),
        options
      )
      .pipe(
        map((response: Response) => {
          let userModel = response.json();
          if (userModel) {
          }
        }),
        catchError(this.handleError)
      );
  }

  getAllUsers(): Observable<Array<User>> {
    let headers = new Headers({
      "Content-Type": "application/json",
      Token: localStorage.getItem("user_token")
    });
    let options = new RequestOptions({ headers: headers });
    return this._httpService
      .get(environment.apiUrl + "/users", options)
      .pipe(
        map((response: Response) => {
          return <Array<User>>response.json();
        }),
        catchError(this.handleError)
      );
  }

  getUserById(userId: number): any {
    let headers = new Headers({
      "Content-Type": "application/json",
      Token: localStorage.getItem("user_token")
    });
    let options = new RequestOptions({ headers: headers });
    return this._httpService
      .get(environment.apiUrl + "users/" + userId, options)
      .pipe(
        map((response: Response) => {
          return response.json();
        }),
        catchError(this.handleError)
      );
  }

  modify(data: any, id: number): any {
    let headers = new Headers({
      "Content-Type": "application/json",
      Token: localStorage.getItem("user_token")
    });
    let options = new RequestOptions({ headers: headers });
    return this._httpService
      .put(
        environment.apiUrl +
        "users/" + id, JSON.stringify(data),
        options
      )
      .pipe(
        map((response: Response) => {
        }),
        catchError(this.handleError)
      );
  }

  deleteUser(userId: any): any {
    let headers = new Headers({
      "Content-Type": "application/json",
      Token: localStorage.getItem("user_token")
    });
    let options = new RequestOptions({ headers: headers });
    return this._httpService
      .delete(environment.apiUrl + "users/" + userId, options)
      .pipe(
        map((response: Response) => {
        }),
        catchError(this.handleError)
      );
  }

  

  getLog(data: any): any {
    let headers = new Headers({
      "Content-Type": "application/json",
      Token: localStorage.getItem("user_token")
    });
    let options = new RequestOptions({ headers: headers });
    return this._httpService
      .post(environment.apiUrl + "users/log", 
      JSON.stringify(data), options)
      .pipe(
        map((response: Response) => {
          return response.json();
        }),
        catchError(this.handleError)
      );
  }

  private handleError(error: Response) {
    if (error.status == 0)
      return throwError(new Error("Ocurrió un error inesperado en el servidor."));
    let body = error["_body"];
    return throwError(new Error(body));
  }
}
