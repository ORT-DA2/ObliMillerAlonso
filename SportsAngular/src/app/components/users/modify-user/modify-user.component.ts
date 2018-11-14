import { Component, Input, NgModule } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { UserService } from '../../../services/user.service';
import { repeat } from 'rxjs/operators';
import { AlertService } from '../../../services/alert.service';

@Component({
  selector: 'modifyUser',
  templateUrl: './modify-user.component.html',
  styleUrls: ['./modify-user.component.css']
})

export class ModifyUserComponent{

  @Input() pageTitle: string;
    registerWidth: number = 250;
    repeatPassword: string;
    id: number;
    user: any;
    data: any = { 'FirstName': "", 'LastName': "", 'Email': "", 'IsAdmin': false, 'UserName': "", 'Password': "" };

    constructor(private _userService: UserService,
        private route: ActivatedRoute,
        private alertService: AlertService,
        private router: Router,
        ) { }

        ngOnInit(): void {
          this.route.params.subscribe(params => {
              this.id = params["id"];
          });
          this.getUserById();
      }
      modify() {
          if (this.data['FirstName'] === "") {
              this.alertService.error("nombre vacio");
          } else if (this.data['LastName'] === "") {
              this.alertService.error("apellido vacio");
          } else if (this.data['Email'] === "") {
              this.alertService.error("email vacio");
          } else if (this.data['IsAdmin'] === "") {
              this.alertService.error("admin vacio");
          } else if (this.data['UserName'] === "") {
              this.alertService.error("usuario vacio");
          } else if (this.data['Password'] === "") {
              this.alertService.error("contraseña vacia");
          } else if (this.data['Password'] != this.repeatPassword) {
              this.alertService.error("las contrasenas no coinciden");
          } else {
              this._userService.modify(this.data, this.id).subscribe(
                  data => {
                      this.alertService.success("Se ha modificado el usuario correctamente!");
                      this.router.navigate(['/users']);
                  },
                  error => {
                      this.alertService.error(error.message);
                  }
              )
          }
      }
  
      deleteUser() {
          this._userService.deleteUser(this.id).subscribe(
              data => {
                  this.router.navigate(['/users']);
              },
              error => {
                  this.alertService.error(error.message);
              })
      }
  
      getUserById(): any {
          this._userService.getUserById(this.id).subscribe(
              obtainedUser => {
                  this.setUserData(obtainedUser);
              }
              ,
              (error: any) => {
                  this.alertService.error(error.message);
              }
          );
      }
  
      setUserData(user) {
          this.data['FirstName'] = user.firstName;
          this.data['LastName'] = user.lastName;
          this.data['UserName'] = user.userName;
          this.data['Email'] = user.email;
          this.data['IsAdmin'] = user.isAdmin;
          this.data['Password'] = user.password;
          this.repeatPassword = user.password;
      }
}
