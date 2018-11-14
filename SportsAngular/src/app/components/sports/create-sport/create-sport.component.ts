import { Component, Input, NgModule } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { SportsService } from '../../../services/sports.service';
import { AlertService } from '../../../services/alert.service';

@Component({
    selector: 'app-create-sport',
    templateUrl: './create-sport.component.html',
    styleUrls: ["create-sport.component.css"]
})

export class CreateSportComponent {

    @Input() pageTitle: string;
    createWidth: number = 250;
    data: any = { 'Name': "", 'Amount': 2 };

    constructor(
        private route: ActivatedRoute,
        private alertService: AlertService,
        private router: Router,
        private sportsService: SportsService) { }

    create() {
        if (this.data['Name'] === "") {
            this.alertService.error("nombre vacio");
        } else if (this.data['Amount'] <2) {
            this.alertService.error("la cantidad debe de ser al menos de 2");
        }  else {
            this.sportsService.create(this.data).subscribe(
                data => {
                    this.router.navigate(['/sports']);
                },
                error => {
                    this.alertService.error(error.message);
                }
            )
        }
    }
}