
import { Component, Input, NgModule } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { CompetitorService } from '../../../services/competitor.service';
import { FavoriteService } from '../../../services/favorite.service';
import { UserService } from '../../../services/user.service';
import { AlertService } from '../../../services/alert.service';
import { Competitor } from "../../../classes/competitor";
import {User} from "../../../classes/user";

@Component({
  selector: 'app-view-favourites',
  templateUrl: './view-favourites.component.html',
  styleUrls: ['./view-favourites.component.css']
})
export class ViewFavouritesComponent {

  @Input() pageTitle: string;
  createWidth: number = 250;
  competitors: Array<Competitor>;
  users: Array<User>;

  constructor(
      private route: ActivatedRoute,
      private alertService: AlertService,
      private router: Router,
      private competitorsService: CompetitorService,
      private userService: UserService,
      private favoriteService: FavoriteService) { }


  ngOnInit() {
    this.competitors = new Array<Competitor>();
    this.users = new Array<User>();
    this.getFavorites();
  }

  getFavorites(){
    this.favoriteService.getFavoriteCompetitors().subscribe(
      ((obtainedCompetitors) => {
          this.competitors = obtainedCompetitors;
      }),
      ((error: any) => {
          this.alertService.error(error.message);
      })
  );
  }

  deleteFavorite(id:number) {
    this.favoriteService.deleteFavoriteCompetitor(id).subscribe(
        data => {
            this.alertService.success("Se ha eliminado el favorito de forma exitosa!");
            this.getFavorites();
        },
        error => {
            this.alertService.error(error.message);
        }
    )
}

}
