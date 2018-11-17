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
import { Match } from "../classes/match";
import { Sport } from "../classes/sport";
import { AlertService } from "./alert.service";

@Injectable({
  providedIn: 'root'
})
export class MatchService {

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
          "matches", JSON.stringify(data),
          options
        )
        .pipe(
          map((response: Response) => {
            let sportModel = response.json();
            if (sportModel) {
            }
          }),
          catchError(this.handleError)
        );
    }
    getAllMatches(): Observable<Array<Sport>> {
      let headers = new Headers({
        "Content-Type": "application/json",
        Token: localStorage.getItem("user_token")
      });
      let options = new RequestOptions({ headers: headers });
      return this._httpService
        .get(environment.apiUrl + "matches/bySport", options)
        .pipe(
          map((response: Response) => {
            return <Array<Sport>>response.json();
          }),
          catchError(this.handleError)
        );
    }

    getAllMatchesBySport(sportId:number): Observable<Array<Sport>> {
      let headers = new Headers({
        "Content-Type": "application/json",
        Token: localStorage.getItem("user_token")
      });
      let options = new RequestOptions({ headers: headers });
      return this._httpService
        .get(environment.apiUrl + "matches/bySport/"+ sportId, options)
        .pipe(
          map((response: Response) => {
            return <Array<Sport>>response.json();
          }),
          catchError(this.handleError)
        );
    }

    
    getAllMatchesByCompetitor(competitorId:number): Observable<Array<Sport>> {
      let headers = new Headers({
        "Content-Type": "application/json",
        Token: localStorage.getItem("user_token")
      });
      let options = new RequestOptions({ headers: headers });
      return this._httpService
        .get(environment.apiUrl + "matches/byCompetitor/"+ competitorId, options)
        .pipe(
          map((response: Response) => {
            return <Array<Sport>>response.json();
          }),
          catchError(this.handleError)
        );
    }

    getMatchById(matchId: number): any {
      let headers = new Headers({
        "Content-Type": "application/json",
        Token: localStorage.getItem("user_token")
      });
      let options = new RequestOptions({ headers: headers });
      return this._httpService
        .get(environment.apiUrl + "matches/" + matchId, options)
        .pipe(
          map((response: Response) => {
            return response.json();
          }),
          catchError(this.handleError)
        );
    }
  
/*
    modifySport(data: any, sportId: number): any {
      console.log('2')
      let headers = new Headers({
        "Content-Type": "application/json",
        Token: localStorage.getItem("user_token")
      });
      let options = new RequestOptions({ headers: headers });
      return this._httpService
        .put(
          environment.apiUrl +
          "sports/" + sportId, JSON.stringify(data),
          options
        )
        .pipe(
          map((response: Response) => {

          }),
          catchError(this.handleError)
        );
    }
  
    deleteSport(sportId: number): any {
      let headers = new Headers({
        "Content-Type": "application/json",
        Token: localStorage.getItem("user_token")
      });
      let options = new RequestOptions({ headers: headers });
      return this._httpService
        .delete(environment.apiUrl + "sports/" + sportId, options)
        .pipe(
          map((response: Response) => {
          }),
          catchError(this.handleError)
        );
    }
    */
  private handleError(error: Response) {
    if (error.status == 0)
      return throwError(new Error("Ocurrió un error inesperado en el servidor."));
    let body = error["_body"];
    return throwError(new Error(body));
  }
}
