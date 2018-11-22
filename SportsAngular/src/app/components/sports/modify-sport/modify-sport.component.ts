import { Component, Input} from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { SportsService } from '../../../services/sports.service';
import { AlertService } from '../../../services/alert.service';
import { CompetitorScore } from 'src/app/classes/competitorScore';

@Component({
  selector: 'app-modify-sport',
  templateUrl: './modify-sport.component.html',
  styleUrls: ['./modify-sport.component.css']
})
export class ModifySportComponent {

  @Input() pageTitle: string;
  createWidth: number = 250;
  id: number;
  sport: any;
  competitors: Array<CompetitorScore>
  data: any = { 'Name': ""};

  constructor(
      private route: ActivatedRoute,
      private alertService: AlertService,
      private router: Router,
      private sportsService: SportsService) { }

      ngOnInit(): void {
        this.route.params.subscribe(params => {
            this.id = params["id"];
        });
        this.getSportById();
    }
  modify() {
    if (this.data['Name'] === "") {
        this.alertService.error("nombre vacio");
    } else {
        this.sportsService.modifySport(this.data, this.id).subscribe(
            data => {
                this.alertService.success("Se ha modificado el deporte correctamente!");
            },
            error => {
                this.alertService.error(error.message);
            }
        )
    }
}

  deleteSport() {
    this.sportsService.deleteSport(this.id).subscribe(
        data => {
          this.router.navigate(['/sports']);
        },
        error => {
            this.alertService.error(error.message);
        })
}

getSportById(): any {
    this.sportsService.getSportById(this.id).subscribe(
        obtainedSport => {
            this.setSportData(obtainedSport);
        }
        ,
        (error: any) => {
            this.alertService.error(error.message);
        }
    );
}

setSportData(sport) {
    this.data['Name'] = sport.name;
    this.data['Amount'] = sport.amount;
    this.getRanking();
}

getRanking(){
    this.sportsService.getRanking(this.id).subscribe(
        obtainedRanking => {
            this.competitors = obtainedRanking;
        }
        ,
        (error: any) => {
            this.alertService.error(error.message);
        }
    );
}

isAdmin(): boolean{
    var admin = localStorage.getItem('isAdmin');
    return admin == true.toString();
  }

}
