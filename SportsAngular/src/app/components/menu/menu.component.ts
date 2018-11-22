import { Component, OnInit } from '@angular/core';

import { LoginService } from '../../services/login.service';
import { AlertService } from '../../services/alert.service';


@Component({
    selector: 'menu',
    templateUrl: './menu.component.html',
    providers: [LoginService, AlertService],
    styleUrls: ['./menu.component.css']
})

export class MenuComponent implements OnInit {
    isClosed: boolean = true;

    constructor(
        private loginService: LoginService,
        private alertService: AlertService
    ) { }

    ngOnInit() {
        
    }

    userLogged(): boolean {
        return localStorage.getItem('user_token') != undefined;
    }

    logout(): void {
        this.loginService.logout().subscribe(
            data => {
                location.reload();
            },
            error => {
                this.alertService.error(error);
            });;
    }

    isAdmin(): boolean{
      var admin = localStorage.getItem('isAdmin');
      return admin == true.toString();
    }
}
