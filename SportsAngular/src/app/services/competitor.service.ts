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

import { Sport } from "../classes/sport";
import { Competitor } from "../classes/competitor";
import { AlertService } from "./alert.service";

@Injectable({
  providedIn: 'root'
})
export class CompetitorService {

  constructor(
    private _httpService: Http,
    private alertService: AlertService
    ) { }

    create(competitorId:number, data: any) {
      let headers = new Headers({
        "Content-Type": "application/json",
        Token: localStorage.getItem("user_token")
      });
      let options = new RequestOptions({ headers: headers });
      return this._httpService
        .post(
          environment.apiUrl +
          "sports/"+competitorId+"/competitors", JSON.stringify(data),
          options
        )
        .pipe(
          map((response: Response) => {
            let competitorModel = response.json();
            if (competitorModel) {
            }
          }),
          catchError(this.handleError)
        );
    }
    
    getCompetitorById(competitorId: number): any {
      let headers = new Headers({
        "Content-Type": "application/json",
        Token: localStorage.getItem("user_token")
      });
      let options = new RequestOptions({ headers: headers });
      return this._httpService
        .get(environment.apiUrl + "competitors/" + competitorId, options)
        .pipe(
          map((response: Response) => {
            return response.json();
          }),
          catchError(this.handleError)
        );
    }
  
    modifyCompetitor(data: any, competitorId: number): any {
      let headers = new Headers({
        "Content-Type": "application/json",
        Token: localStorage.getItem("user_token")
      });
      let options = new RequestOptions({ headers: headers });
      return this._httpService
        .put(
          environment.apiUrl +
          "competitors/" + competitorId, JSON.stringify(data),
          options
        )
        .pipe(
          map((response: Response) => {
          }),
          catchError(this.handleError)
        );
    }
  
    deleteCompetitor(competitorId: number): any {
      let headers = new Headers({
        "Content-Type": "application/json",
        Token: localStorage.getItem("user_token")
      });
      let options = new RequestOptions({ headers: headers });
      return this._httpService
        .delete(environment.apiUrl + "competitors/" + competitorId, options)
        .pipe(
          map((response: Response) => {
          }),
          catchError(this.handleError)
        );
    }

    getAllCompetitors(): Observable<Array<Competitor>> {
      let headers = new Headers({
        "Content-Type": "application/json",
        Token: localStorage.getItem("user_token")
      });
      let options = new RequestOptions({ headers: headers });
      return this._httpService
        .get(environment.apiUrl + "competitors", options)
        .pipe(
          map((response: Response) => {
            return <Array<Competitor>>response.json();
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
