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

import {LoginService} from './services/login.service';
import { AlertService } from './services/alert.service';

const appRoutes: Routes = [
  { path: 'favourites',component: FavouritesComponent},
]

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    AlertComponent,
    FavouritesComponent,
    MenuComponent
  ],
  imports: [
    BrowserModule,
    HttpModule,
    AppRoutingModule,
    FormsModule,
    RouterModule.forRoot(appRoutes)
  ],
  providers: [LoginService, AlertService],
  bootstrap: [AppComponent]
})
export class AppModule { }
