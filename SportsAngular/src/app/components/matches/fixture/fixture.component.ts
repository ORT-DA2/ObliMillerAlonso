import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { MatchService } from '../../../services/match.service';
import { AlertService } from '../../../services/alert.service';
import { SportsService } from '../../../services/sports.service';
import {Fixture} from "../../../classes/fixture";
import {Sport} from "../../../classes/sport";

@Component({
  selector: 'app-fixture',
  templateUrl: './fixture.component.html',
  styleUrls: ['./fixture.component.css']
})
export class FixtureComponent {
  sportsArray: Array<Sport>;
  fixtures: Array<Fixture>;
  comments: any;
  data: any = { 'Pos': "", 'SportId': "",'Date': ""};

  constructor(
    private route: ActivatedRoute,
    private alertService: AlertService,
    private sportsService: SportsService,
    private router: Router,
    private matchService: MatchService) { }

    ngOnInit(): void {
      this.getSports();
      this.getFixtures();
    }

    getFixtures(){
      this.fixtures = new Array<Fixture>();
      this.matchService.getAllFixtures().subscribe(
          ((obtainedFixtures) => {
            for (var i = 0; i < obtainedFixtures.length; i++) {
              this.fixtures[i] = new Fixture(i,obtainedFixtures[i]); 
            }
          }),
          ((error: any) => {
              this.alertService.error(error.message);
          })
      );

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
    generateFixture(id:number): void {
      this.data['Pos'] = id;
      this.matchService.generateFixture(this.data).subscribe(
          ((data) => {
            this.router.navigate(['/matches']);
          }),
          ((error: any) => {
              this.alertService.error(error.message);
          })
      );
    }


}