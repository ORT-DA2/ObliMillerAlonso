import { Component, Input, NgModule } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { SportsService } from '../../../services/sports.service';
import { AlertService } from '../../../services/alert.service';
import { Sport } from "../../../classes/sport";

@Component({
  selector: 'app-view-sports',
  templateUrl: './view-sports.component.html',
  styleUrls: ['./view-sports.component.css']
})
export class ViewSportsComponent{

 
  @Input() pageTitle: string;
  createWidth: number = 250;
  sports: Array<Sport>;
  sportService: any;

  constructor(
      private route: ActivatedRoute,
      private alertService: AlertService,
      private router: Router,
      private sportsService: SportsService) { }


      ngOnInit(): void {
        this.sports = new Array<Sport>();

        this.sportsService.getAllSports().subscribe(
            ((obtainedSports) => {
                this.sports = obtainedSports;
            }),
            ((error: any) => {
                this.alertService.error(error.message);
            })
        );
    }
}
