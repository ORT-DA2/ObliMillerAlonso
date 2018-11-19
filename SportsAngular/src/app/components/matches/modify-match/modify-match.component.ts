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
  selector: 'app-modify-match',
  templateUrl: './modify-match.component.html',
  styleUrls: ['./modify-match.component.css']
})
export class ModifyMatchComponent {
  id: number;
  comments:Array<Comment>;
  sportsArray:Array<Sport>;
  comment:Comment;
  selectedSport: Sport;
  sportName: string;
  competitors:Array<number>;
  competitorScores:Array<CompetitorScoreDTO>;
  competitorsArray:Array<Competitor>;
  date:string;
  data: any = { 'SportId': "", 'Competitors': "",'Date': ""};

  constructor(
    private route: ActivatedRoute,
    private alertService: AlertService,
    private router: Router,
    private sportsService: SportsService,
    private matchService: MatchService,) { }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
        this.id = params["id"];
    });
    this.comment = new Comment("");
    this.getMatchById();
  }
  
  getMatchById(): any {
  this.matchService.getMatchById(this.id).subscribe(
      obtainedMatch => {
          this.setMatchData(obtainedMatch);
      }
      ,
      (error: any) => {
          this.alertService.error(error.message);
      }
    );
  }

  setMatchData(match) {
    this.data['SportId'] = match.sport.id;
    this.data['Competitors'] = match.competitors;
    this.id = match.id;
    this.comments = match.comments;
    this.competitorScores = match.competitors;
    this.competitors = Array<number>(match.competitors.length).fill(0);
    for (var i = 0; i < this.competitors.length; i++) {
      this.competitors[i] = i; 
    }
    this.setDate(match.date);
    this.getSportById(match.sport.id);
    
  }

  setDate(date:string){
    let datetime = date.split(" ");
    let splitDate = datetime[0].split("/");
    let splitTime = datetime[1].split(":");
    for (var i = 0; i < splitDate.length; i++) {
      if(splitDate[i].length<2){
        splitDate[i] = "0"+splitDate[i];
      }
    }
    for (var i = 0; i < splitTime.length; i++) {
      if(splitTime[i].length<2){
        splitTime[i] = "0"+splitTime[i];
      }
    }
    this.date =  splitDate[2]+"-"+splitDate[1]+"-"+splitDate[0]+"T"+splitTime[0]+":"+splitTime[1];
  }

  getSportById(id:number){
    this.sportsService.getSportById(id).subscribe(
      obtainedSport => {
          this.selectedSport = obtainedSport;
          this.sportName = obtainedSport.name;
          this.competitorsArray = this.selectedSport.competitors;
      },
      (error: any) => {
          this.alertService.error(error.message);
      }
    );
  }

  modify() {
    if (this.selectedSport == null) {
      this.alertService.error("eliga un deporte");
    } else if (this.date == null) {
        this.alertService.error("eliga una fecha");
    } else {
      this.data.SportId = this.selectedSport.id;
      this.data.Competitors = this.competitorScores;
      this.data.Date = this.date;
        this.matchService.modifyMatch(this.data, this.id).subscribe(
            data => {
                this.alertService.success("Se ha modificado el deporte correctamente!");
            },
            error => {
                this.alertService.error(error.message);
            }
        )
    }
  }
  
  deleteMatch() {
    this.matchService.deleteMatch(this.id).subscribe(
        data => {
          this.router.navigate(['/matches']);
        },
        error => {
            this.alertService.error(error.message);
        })
  }

  createComment(){
    this.matchService.addComment(this.id,this.comment).subscribe(
      data => {
        this.getMatchById();
      },
      error => {
          this.alertService.error(error.message);
      })
  }
  
  isAdmin(): boolean{
    var admin = localStorage.getItem('isAdmin');
    return admin == true.toString();
  }

}
