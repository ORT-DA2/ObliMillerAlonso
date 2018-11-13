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


import {LoginService} from './services/login.service';
import { AlertService } from './services/alert.service';
import { UserService } from './services/user.service';



const appRoutes: Routes = [
  { path: 'favourites',component: FavouritesComponent},
  { path: 'users/createUser',component: CreateUserComponent},
]

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    AlertComponent,
    FavouritesComponent,
    MenuComponent,
    CreateUserComponent
  ],
  imports: [
    BrowserModule,
    HttpModule,
    AppRoutingModule,
    FormsModule,
    RouterModule.forRoot(appRoutes)
  ],
  providers: [LoginService, AlertService, UserService],
  bootstrap: [AppComponent]
})
export class AppModule { }
