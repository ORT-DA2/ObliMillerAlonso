import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  showLoader: boolean;
  title = 'Sports Tracker';

  
userLogged(): boolean {
  return localStorage.getItem("user_token")!=null;
}
}
