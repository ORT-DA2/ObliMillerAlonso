import { BrowserModule } from '@angular/platform-browser';
import { NgModule,  } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { RouterModule, Routes} from '@angular/router';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { CalendarModule, DateAdapter } from 'angular-calendar';
import { adapterFactory } from 'angular-calendar/date-adapters/date-fns';
import { CommonModule } from '@angular/common';
import { NgbModalModule } from '@ng-bootstrap/ng-bootstrap';
import { FlatpickrModule } from 'angularx-flatpickr';

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
import { CreateMatchComponent } from './components/matches/create-match/create-match.component';
import { ModifyMatchComponent } from './components/matches/modify-match/modify-match.component';
import { ViewMatchesComponent } from './components/matches/view-matches/view-matches.component';

import { LoginService } from './services/login.service';
import { AlertService } from './services/alert.service';
import { UserService } from './services/user.service';
import { SportsService } from './services/sports.service';
import { CompetitorService } from './services/competitor.service';
import { FavoriteService } from './services/favorite.service';
import { MatchService } from './services/match.service';
import { ViewLogComponent } from './components/users/view-log/view-log.component';
import { FixtureComponent } from './components/matches/fixture/fixture.component';
import { UserGuard } from './guards/user.guard';
import { AdminGuard } from './guards/admin.guard';
import { ListCommentsComponent } from './components/comments/list-comments/list-comments.component';
import { ListRankingComponent } from './components/competitors/list-ranking/list-ranking.component';


const appRoutes: Routes = [
  { path: 'users/create',component: CreateUserComponent, canActivate:[AdminGuard]},
  { path: 'users',component: ViewUsersComponent, canActivate:[AdminGuard]},
  { path: 'log',component: ViewLogComponent, canActivate:[AdminGuard]},
  { path: 'users/:id',component: ModifyUserComponent, canActivate:[AdminGuard]},
  { path: 'sports/create',component: CreateSportComponent, canActivate:[AdminGuard]},
  { path: 'sports',component: ViewSportsComponent, canActivate:[UserGuard]},
  { path: 'sports/:id',component: ModifySportComponent, canActivate:[UserGuard]},
  { path: 'competitors/create',component: CreateCompetitorComponent, canActivate:[AdminGuard]},
  { path: 'competitors',component: ViewCompetitorsComponent},
  { path: 'competitors/:id',component: ModifyCompetitorsComponent, canActivate:[UserGuard]},
  { path: 'favourites', component: ViewFavouritesComponent, canActivate:[UserGuard]},
  { path: 'matches/create',component: CreateMatchComponent, canActivate:[AdminGuard]},
  { path: 'fixtures',component: FixtureComponent, canActivate:[AdminGuard]},
  { path: 'matches/:id',component: ModifyMatchComponent, canActivate:[UserGuard]},
  { path: 'matches',component: ViewMatchesComponent, canActivate:[UserGuard]},
  { path: '', component: ViewCommentsComponent},
  { path: '**', component: ViewCommentsComponent},
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
    ViewCommentsComponent,
    CreateMatchComponent,
    ModifyMatchComponent,
    ViewMatchesComponent,
    ViewLogComponent,
    FixtureComponent,
    ListCommentsComponent,
    ListRankingComponent
  ],
  imports: [
    BrowserModule,
    HttpModule,
    AppRoutingModule,
    FormsModule,
    RouterModule.forRoot(appRoutes),
    BrowserAnimationsModule,
    CommonModule,
    FormsModule,
    NgbModalModule,
    FlatpickrModule.forRoot(),
    CalendarModule.forRoot({
      provide: DateAdapter,
      useFactory: adapterFactory
    })
  ],
  providers: [LoginService, AlertService, SportsService, UserService, CompetitorService, MatchService, FavoriteService],
  bootstrap: [AppComponent]
})
export class AppModule { }
