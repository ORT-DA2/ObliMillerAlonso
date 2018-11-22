import { Component } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { UserService } from '../../../services/user.service';
import { AlertService } from '../../../services/alert.service';


@Component({
  selector: 'app-view-log',
  templateUrl: './view-log.component.html',
  styleUrls: ['./view-log.component.css']
})
export class ViewLogComponent {
  logs: any;
  startDate: Date = new Date(0);
  endDate: Date = new Date();
  data:any = {"StartDate" : "", "FinishDate" : ''}

  constructor(
    private alertService: AlertService,
    private userService: UserService) { }

    ngOnInit(): void {
     this.refreshLog();
    }

    refreshLog(){
      let valueStart = new Date(this.startDate).valueOf();
      let valueEnd = new Date(this.endDate).valueOf();
      if(valueStart>valueEnd){
        this.alertService.error("la fecha de inicio debe ser menor a la de fin");
      }else{
        this.data["StartDate"] = this.startDate;
        this.data["FinishDate"] = this.endDate
        this.userService.getLog(this.data).subscribe(
            ((obtainedComments) => {
                this.logs = obtainedComments;
            }),
            ((error: any) => {
                this.alertService.error(error.message);
            })
        );
      }
    }


}
