import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, Router } from '@angular/router';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AdminGuard implements CanActivate {
  constructor(private router: Router) { }
  canActivate(route: ActivatedRouteSnapshot): boolean {
    if (this.userIsAdmin()) {
        return true;
    }
    this.router.navigate(['']);
    return false;
}
userIsAdmin(): boolean {
  let isAdmin = localStorage.getItem('isAdmin');
  let token = localStorage.getItem('user_token');
  if (isAdmin&&token){
      return JSON.parse(isAdmin);
  }
  return undefined;
}
}
