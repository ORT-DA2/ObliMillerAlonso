import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, Router } from '@angular/router';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserGuard implements CanActivate {
  
  constructor(private router: Router) { }
  
  canActivate(route: ActivatedRouteSnapshot): boolean {
    let token = localStorage.getItem('user_token');
    if (token){
        return true;
    }
    this.router.navigate(['']);
    return false;
  } 
}
