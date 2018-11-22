import { Component, Input, NgModule } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { SportsService } from '../../../services/sports.service';
import { CompetitorService } from '../../../services/competitor.service';
import { AlertService } from '../../../services/alert.service';
import { Sport } from '../../../classes/sport';

@Component({
  selector: 'app-create-competitor',
  templateUrl: './create-competitor.component.html',
  styleUrls: ['./create-competitor.component.css']
})
export class CreateCompetitorComponent  {
  selectedFile: File = null;
  reader:FileReader = new FileReader;
  sportsArray:Array<Sport>;
  sportId: number = 0;

  @Input() pageTitle: string;
  createWidth: number = 250;
  data: any = { 'Name': "", 'Picture': ""};

  constructor(
    private alertService: AlertService,
    private router: Router,
    private sportsService: SportsService,
    private competitorService: CompetitorService
    ) { }

  ngOnInit() {
    this.getSports();
  }

  onFileSelected(event){
    this.selectedFile = <File>event.target.files[0];
    this.reader.readAsDataURL(this.selectedFile);
    this.reader.onloadend = () => this.data.Picture = this.reader.result;
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
    if (this.data['Name'] === "") {
        this.alertService.error("nombre vacio");
    }  else if (this.sportId == 0) {
        this.alertService.error("eliga un deporte");
    } else {
        this.competitorService.create(this.sportId, this.data).subscribe(
            data => {
                this.router.navigate(['/competitors']);
            },
            error => {
                this.alertService.error(error.message);
            }
        );
    }
  }

}
