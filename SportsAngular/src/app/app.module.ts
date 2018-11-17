import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { RouterModule, Routes} from '@angular/router';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginComponent } from './components/login/login.component';
import { AlertComponent } from './components/alert/alert.component';
import { MenuComponent } from './components/menu/menu.component';
import { CreateUserComponent } from './components/users/create-user/create-user.component';
import { CreateSportComponent } from './components/sports/create-sport/create-sport.component';
import { ViewUsersComponent } from './components/users/view-users/view-users.component';
import { ModifyUserComponent } from './components/users/modify-user/modify-user.component';
import { ViewSportsComponent } from './components/sports/view-sports/view-sports.component';
import { ModifySportComponent } from './components/sports/modify-sport/modify-sport.component';
import { CreateCompetitorComponent } from './components/competitors/create-competitor/create-competitor.component';
import { ViewCompetitorsComponent } from './components/competitors/view-competitors/view-competitors.component';
import { ModifyCompetitorsComponent } from './components/competitors/modify-competitors/modify-competitors.component';
import { ViewFavouritesComponent } from './components/favourites/view-favourites/view-favourites.component';
import { ViewCommentsComponent } from './components/favourites/view-comments/view-comments.component';




import {LoginService} from './services/login.service';
import { AlertService } from './services/alert.service';
import { UserService } from './services/user.service';
import { SportsService } from './services/sports.service';
import { CompetitorService } from './services/competitor.service';
import { FavoriteService } from './services/favorite.service';


const appRoutes: Routes = [
  { path: 'users/create',component: CreateUserComponent},
  { path: 'users',component: ViewUsersComponent},
  { path: 'users/modify/:id',component: ModifyUserComponent},
  { path: 'sports/create',component: CreateSportComponent},
  { path: 'sports',component: ViewSportsComponent},
  { path: 'sports/modify/:id',component: ModifySportComponent},
  { path: 'competitors/create',component: CreateCompetitorComponent},
  { path: 'competitors',component: ViewCompetitorsComponent},
  { path: 'competitors/modify/:id',component: ModifyCompetitorsComponent},
  { path: 'favourites', component: ViewFavouritesComponent},
  { path: '**', component: ViewCommentsComponent}
]

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    AlertComponent,
    MenuComponent,
    CreateUserComponent,
    CreateSportComponent,
    ViewUsersComponent,
    ModifyUserComponent,
    ViewSportsComponent,
    ModifySportComponent,
    CreateCompetitorComponent,
    ViewCompetitorsComponent,
    ModifyCompetitorsComponent,
    ViewFavouritesComponent,
    ViewCommentsComponent
  ],
  imports: [
    BrowserModule,
    HttpModule,
    AppRoutingModule,
    FormsModule,
    RouterModule.forRoot(appRoutes)
  ],
  providers: [LoginService, AlertService, SportsService, UserService, CompetitorService, FavoriteService],
  bootstrap: [AppComponent]
})
export class AppModule { }
