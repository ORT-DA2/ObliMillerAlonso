import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import {Comment} from "../../../classes/comment";
import { CompetitorService } from '../../../services/competitor.service';
import { FavoriteService } from '../../../services/favorite.service';
import { UserService } from '../../../services/user.service';
import { AlertService } from '../../../services/alert.service';
import { Competitor } from "../../../classes/competitor";
import {User} from "../../../classes/user";

@Component({
  selector: 'app-view-comments',
  templateUrl: './view-comments.component.html',
  styleUrls: ['./view-comments.component.css']
})
export class ViewCommentsComponent{

  comments: any;

  constructor(
    private route: ActivatedRoute,
    private alertService: AlertService,
    private router: Router,
    private favoriteService: FavoriteService) { }

    ngOnInit(): void {
      this.favoriteService.getAllComments().subscribe(
          ((obtainedComments) => {
              this.comments = obtainedComments;
          }),
          ((error: any) => {
              this.alertService.error(error.message);
          })
      );
  }


}
