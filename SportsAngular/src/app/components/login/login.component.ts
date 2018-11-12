import { Component, Input, NgModule } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { LoginService } from '../../services/login.service';
import { AlertService } from '../../services/alert.service';

@Component({
    selector: 'pm-login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.css']
})

export class LoginComponent {

    @Input() pageTitle: string;
    loginWidth: number = 250;


    data = { 'UserName': "", 'Password': "" };

    constructor(
        private route: ActivatedRoute,
        private alertService: AlertService,
        private router: Router,
        private loginService: LoginService) { }

    onSubmit() {
        if (this.data['UserName'] === "") {
            this.alertService.error("usuario vacio");
        } else if (this.data['Password'] === "") {
            this.alertService.error("contraseña vacia");
        } else {
            
            this.loginService.login(this.data['UserName'], this.data['Password']).subscribe(
                data => {
                    var da = this.loginService.isAdminUser().subscribe(data => {
                    },
                    error => {
                        this.alertService.error(error.message);
                    });
                    this.router.navigate(['favourites']);
                },
                error => {
                    this.alertService.error(error.message);
                }
            )
        }
    }

}