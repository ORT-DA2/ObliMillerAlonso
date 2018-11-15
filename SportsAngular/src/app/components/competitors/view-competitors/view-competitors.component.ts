import { Component, Input, NgModule } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { CompetitorService } from '../../../services/competitor.service';
import { AlertService } from '../../../services/alert.service';
import { Competitor } from "../../../classes/competitor";

@Component({
  selector: 'app-view-competitors',
  templateUrl: './view-competitors.component.html',
  styleUrls: ['./view-competitors.component.css']
})
export class ViewCompetitorsComponent{

 
  @Input() pageTitle: string;
  createWidth: number = 250;
  competitors: Array<Competitor>

  constructor(
      private route: ActivatedRoute,
      private alertService: AlertService,
      private router: Router,
      private competitorsService: CompetitorService) { }


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
}
