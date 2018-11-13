import { Component, Input, NgModule } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { UserService } from '../../../services/user.service';
import { AlertService } from '../../../services/alert.service';

@Component({
    selector: 'createUser',
    templateUrl: './create-user.component.html',
    styleUrls: ["create-user.component.css"]
})

export class CreateUserComponent {

    @Input() pageTitle: string;
    createWidth: number = 250;
    repeatPassword: string;
    data: any = { 'FirstName': "", 'LastName': "", 'Email': "", 'IsAdmin': false, 'UserName': "", 'Password': "" };

    constructor(private _userService: UserService,
        private route: ActivatedRoute,
        private alertService: AlertService,
        private router: Router,
        ) { }

    create() {
        if (this.data['FirstName'] === "") {
            this.alertService.error("nombre no puede ser vacio");
        } else if (this.data['LastName'] === "") {
            this.alertService.error("apellido no puede ser vacio");
        } else if (this.data['Email'] === "") {
            this.alertService.error("email no puede ser vacio");
        } else if (this.data['IsAdmin'] === "") {
            this.alertService.error("admin vacio");
        } else if (this.data['UserName'] === "") {
            this.alertService.error("usuario no puede ser vacio");
        } else if (this.data['Password'] === "") {
            this.alertService.error("contraseña no puede ser vacia");
        } else if (this.data['Password'] != this.repeatPassword) {
            this.alertService.error("las contrasenas no coinciden");
        } else {
            this._userService.create(this.data).subscribe(
                data => {
                    this.router.navigate(['/users']);
                },
                error => {
                    this.alertService.error(error.message);
                }
            )
        }
    }
}