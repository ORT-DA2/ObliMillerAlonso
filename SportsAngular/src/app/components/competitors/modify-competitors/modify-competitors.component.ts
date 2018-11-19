import { Component, Input, NgModule } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { SportsService } from '../../../services/sports.service';
import { CompetitorService } from '../../../services/competitor.service';
import { AlertService } from '../../../services/alert.service';
import { Sport } from '../../../classes/sport';
import { PublicFeature } from '@angular/core/src/render3';
import { $ } from 'protractor';

@Component({
  selector: 'app-modify-competitors',
  templateUrl: './modify-competitors.component.html',
  styleUrls: ['./modify-competitors.component.css']
})
export class ModifyCompetitorsComponent {
  @Input() pageTitle: string;
  createWidth: number = 250;
  id: number;
  selectedFile: File = null;
  reader:FileReader = new FileReader;
  sport:Sport;
  competitor: any;
  data: any = { 'Name': "", 'Picture': ""};

  constructor(
    private route: ActivatedRoute,
    private alertService: AlertService,
    private router: Router,
    private competitorService: CompetitorService) { }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
        this.id = params["id"];
    });
    this.getCompetitorById();
}


onFileSelected(event){
  this.selectedFile = <File>event.target.files[0];
  this.reader.readAsDataURL(this.selectedFile);
  this.reader.onloadend = () => this.data.Picture = this.reader.result;
}

modify() {
  if (this.data['Name'] === "") {
    this.alertService.error("nombre vacio");
    } else if (this.data['Picture'] === "") {
    this.alertService.error("eliga una imagen");
  }  else {
      this.competitorService.modifyCompetitor(this.data, this.id).subscribe(
          data => {
              this.alertService.success("Se ha modificado el deporte correctamente!");
          },
          error => {
              this.alertService.error(error.message);
          }
      )
  }
}

deleteCompetitor() {
  this.competitorService.deleteCompetitor(this.id).subscribe(
      data => {
        this.router.navigate(['/competitor']);
      },
      error => {
          this.alertService.error(error.message);
      })
}

getCompetitorById(): any {
  this.competitorService.getCompetitorById(this.id).subscribe(
      obtainedCompetitor => {
          this.setCompetitorData(obtainedCompetitor);
      }
      ,
      (error: any) => {
          this.alertService.error(error.message);
      }
  );
}

setCompetitorData(competitor) {
  this.data['Name'] = competitor.name;
  this.data['Picture'] = competitor.picture;
  this.sport = competitor.sport;
  this.id = competitor.id;
}

isAdmin(): boolean{
  var admin = localStorage.getItem('isAdmin');
  return admin == true.toString();
}

}
