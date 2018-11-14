import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';
import { UserService } from '../../../services/user.service';
import { AlertService } from '../../../services/alert.service';
import { User } from '../../../classes/user';

@Component({
  selector: 'app-view-users',
  templateUrl: './view-users.component.html',
  styleUrls: ['./view-users.component.css']
})
export class ViewUsersComponent{

  @Input() pageTitle: string;
  users: Array<User>;

  constructor(private _userService: UserService,
    private router: Router,
    private alertService: AlertService,
      ) { }

      ngOnInit(): void {
        this.users = new Array<User>();

        this._userService.getAllUsers().subscribe(
            ((obtainedUsers) => {
                this.users = obtainedUsers;
            }),
            ((error: any) => {
                this.alertService.error(error.message);
            })
        );
    }

}
