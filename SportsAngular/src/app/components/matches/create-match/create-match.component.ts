import { Component, Input, NgModule } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { SportsService } from '../../../services/sports.service';
import { MatchService } from '../../../services/match.service';
import { AlertService } from '../../../services/alert.service';
import { Sport } from '../../../classes/sport';
import { Competitor } from '../../../classes/competitor';
import { CompetitorScoreDTO } from '../../../classes/competitorScoreDTO';
import { PublicFeature } from '@angular/core/src/render3';
import { $ } from 'protractor';
import { Match } from 'src/app/classes/match';

@Component({
  selector: 'app-create-match',
  templateUrl: './create-match.component.html',
  styleUrls: ['./create-match.component.css']
})
export class CreateMatchComponent  {
  sportsArray:Array<Sport>;
  selectedSport: Sport;
  competitors:Array<number>;
  competitorScores:Array<CompetitorScoreDTO>;
  competitorsArray:Array<Competitor>;
  date:Date;
  data: any = { 'SportId': "", 'Competitors': "",'Date': ""};

  @Input() pageTitle: string;
  createWidth: number = 250;

  constructor(
    private route: ActivatedRoute,
    private alertService: AlertService,
    private router: Router,
    private sportsService: SportsService,
    private matchService: MatchService,
    ) { }

  ngOnInit() {
    this.getSports();
  }

  onSportSelected(value){
    this.sportsArray.forEach(sport => {
      if (sport.id==this.data.SportId){
        this.selectedSport = sport;
      }
    });
    this.competitorsArray = this.selectedSport.competitors;
    this.competitors = Array<number>(this.selectedSport.amount).fill(0);
    this.competitorScores = Array<CompetitorScoreDTO>(this.selectedSport.amount).fill(new CompetitorScoreDTO(0,0));
    for (var i = 0; i < this.competitors.length; i++) {
      this.competitors[i] = i; 
  }
  }

  getSports(): any {
    this.sportsArray = new Array<Sport>();

    this.sportsService.getAllSports().subscribe(
        ((obtainedSports) => { this.sportsArray = obtainedSports }),
        ((error: any) => {
            this.alertService.error(error.message);
        })
    );
  }
  create() {
    if (this.selectedSport === null) {
        this.alertService.error("eliga un deporte");
    } else if (this.competitors === null) {
        this.alertService.error("eliga una competidores");
    } else if (this.date === null) {
        this.alertService.error("eliga una fecha");
    } else {
      this.data.SportId = this.selectedSport.id;
      this.data.Competitors = this.competitors;
      this.data.Date = this.date;
        this.matchService.create(this.data).subscribe(
            data => {
                //this.router.navigate(['/competitors']);
            },
            error => {
                this.alertService.error(error.message);
            }
        );
    }
  }
}
