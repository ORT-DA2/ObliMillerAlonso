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

  private handleError(error: Response) {
    if (error.status == 0)
      return throwError(new Error("Ocurrió un error inesperado en el servidor."));
    let body = error["_body"];
    return throwError(new Error(body));
  }
}
