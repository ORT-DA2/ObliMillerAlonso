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
export class LoginService {

  public token: string;
  constructor(
    private _httpService: Http,
    private alertService: AlertService) { }

    login(username: string, password: string) {
      let headers = new Headers({ "Content-Type": "application/json" });
      let options = new RequestOptions({ headers: headers });
      return this._httpService
        .put(
          environment.apiUrl +
          "login?username=" +
          username +
          "&password=" +
          password,
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
    
  private handleError(error: Response) {
    if (error.status == 0)
      return throwError(new Error("Ocurrió un error inesperado en el servidor."));
    return throwError(new Error(error.json()));
  }
}
