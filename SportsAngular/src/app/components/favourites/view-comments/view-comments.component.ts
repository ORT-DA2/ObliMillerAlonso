import { Component } from '@angular/core';
import { FavoriteService } from '../../../services/favorite.service';
import { AlertService } from '../../../services/alert.service';

@Component({
  selector: 'app-view-comments',
  templateUrl: './view-comments.component.html',
  styleUrls: ['./view-comments.component.css']
})
export class ViewCommentsComponent{

  comments: any;

  constructor(
    private alertService: AlertService,
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
