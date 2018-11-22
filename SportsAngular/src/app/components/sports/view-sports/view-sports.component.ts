import { Component, Input } from '@angular/core';
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
      private alertService: AlertService,
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
