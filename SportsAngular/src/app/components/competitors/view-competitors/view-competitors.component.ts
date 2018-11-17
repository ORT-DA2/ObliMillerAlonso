import { Component, Input, NgModule } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { CompetitorService } from '../../../services/competitor.service';
import { FavoriteService } from '../../../services/favorite.service';

import { AlertService } from '../../../services/alert.service';
import { Competitor } from "../../../classes/competitor";

@Component({
  selector: 'app-view-competitors',
  templateUrl: './view-competitors.component.html',
  styleUrls: ['./view-competitors.component.css']
})
export class ViewCompetitorsComponent{

    public filter = { 'filterName':"", 'ascendingOrder':true};


  competitorId:number;
 
  @Input() pageTitle: string;
  createWidth: number = 250;
  competitors: Array<Competitor>

  data: any = { 'Name': "", 'Picture': "" };
  favData : any =  {'Id' : ""};


  constructor(
      private route: ActivatedRoute,
      private alertService: AlertService,
      private router: Router,
      private competitorsService: CompetitorService,
      private favoriteService: FavoriteService) { }


      ngOnInit(): void {
        this.competitors = new Array<Competitor>();

        this.competitorsService.getAllCompetitors().subscribe(
            ((obtainedCompetitors) => {
                this.competitors = obtainedCompetitors;
            }),
            ((error: any) => {
                this.alertService.error(error.message);
            })
        );
    }

    createfavorite(id:number) {
        this.favData.Id = id;
        this.favoriteService.createfavorite(this.favData).subscribe(
            data => {
                this.alertService.success("Se ha agregado el favorito de forma exitosa!");
            },
            error => {
                this.alertService.error(error.message);
            }
        )
    }

    applyFilters(){
        
        this.competitorsService.filterByName(this.filter['filterName'], this.filter['ascendingOrder']?"asc":"desc").subscribe(
          obtainedDocuments => {
            this.competitors = obtainedDocuments;
          },
          (error: any) => {
            this.alertService.error(error.message);
          }
        );
      }
}
