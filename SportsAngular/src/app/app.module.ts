import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { RouterModule, Routes} from '@angular/router';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginComponent } from './components/login/login.component';
import { AlertComponent } from './components/alert/alert.component';
import { FavouritesComponent } from './components/favourites/favourites.component';
import { MenuComponent } from './components/menu/menu.component';
import { CreateUserComponent } from './components/users/create-user/create-user.component';

import { CreateSportComponent } from './components/sports/create-sport/create-sport.component';

import {LoginService} from './services/login.service';
import { AlertService } from './services/alert.service';
import { UserService } from './services/user.service';
import { SportsService } from './services/sports.service';



const appRoutes: Routes = [
  { path: 'favourites',component: FavouritesComponent},
  { path: 'users/createUser',component: CreateUserComponent},
  { path: 'sports/createSport',component: CreateSportComponent},
]

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    AlertComponent,
    FavouritesComponent,
    MenuComponent,
    CreateUserComponent,
    CreateSportComponent
  ],
  imports: [
    BrowserModule,
    HttpModule,
    AppRoutingModule,
    FormsModule,
    RouterModule.forRoot(appRoutes)
  ],
  providers: [LoginService, AlertService, SportsService, UserService],
  bootstrap: [AppComponent]
})
export class AppModule { }
