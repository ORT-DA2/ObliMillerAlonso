import { Injectable } from '@angular/core';
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
import { Competitor } from "../classes/competitor";
import { AlertService } from "./alert.service";

@Injectable({
  providedIn: 'root'
})
export class FavoriteService {

  constructor(
    private _httpService: Http,
    private alertService: AlertService,
    ) { }

    createfavorite(data: any) {
      let headers = new Headers({
        "Content-Type": "application/json",
        Token: localStorage.getItem("user_token")
      });
      let options = new RequestOptions({ headers: headers });
      return this._httpService
        .post(
          environment.apiUrl +
          "users/favoriteCompetitors", JSON.stringify(data),
          options
        )
        .pipe(
          map((response: Response) => {
          }),
          catchError(this.handleError)
        );
    }

    getFavoriteCompetitors(): Observable<Array<Competitor>> {
      let headers = new Headers({
        "Content-Type": "application/json",
        Token: localStorage.getItem("user_token")
      });
      let options = new RequestOptions({ headers: headers });
      return this._httpService
        .get(environment.apiUrl + "users/favoriteCompetitors", options)
        .pipe(
          map((response: Response) => {
            return <Array<Competitor>>response.json();
          }),
          catchError(this.handleError)
        );
    }

    deleteFavoriteCompetitor(competitorId: number): any {
      let headers = new Headers({
        "Content-Type": "application/json",
        Token: localStorage.getItem("user_token")
      });
      let options = new RequestOptions({ headers: headers });
      return this._httpService
        .delete(environment.apiUrl + "users/favorite/" + competitorId, options)
        .pipe(
          map((response: Response) => {
          }),
          catchError(this.handleError)
        );
    }

    getAllComments():  Observable<Array<Comment>> {
      let headers = new Headers({
        "Content-Type": "application/json",
        Token: localStorage.getItem("user_token")
      });
      let options = new RequestOptions({ headers: headers });
      return this._httpService
        .get(environment.apiUrl + "users/favoriteComments", options)
        .pipe(
          map((response: Response) => {
            return <Array<Comment>>response.json();
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
