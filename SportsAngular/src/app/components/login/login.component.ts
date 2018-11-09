import { Component, Input, NgModule } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { LoginService } from '../../services/login.service';
/*import { AlertService } from '../../services/alert.service';*/

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
      /* private alertService: AlertService,*/
        private route: ActivatedRoute,
        private router: Router,
        private loginService: LoginService) { }

    onSubmit() {
        if (this.data['UserName'] === "") {
           /* this.alertService.error("usuario vacio");*/
        } else if (this.data['Password'] === "") {
            /*this.alertService.error("contraseña vacia");*/
        } else {
            
            this.loginService.login(this.data['UserName'], this.data['Password']).subscribe(
                data => {
                    this.router.navigate(['favourites']);
                },
                error => {
                   /* this.alertService.error(error.message);*/
                }
            )
        }
    }

}