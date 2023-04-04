import { Injectable } from '@angular/core';
import {
  Router,
  CanActivate,
  ActivatedRouteSnapshot,
  RouterStateSnapshot,
} from '@angular/router';

import { AuthService } from '../service/auth.service';

@Injectable({
  providedIn: 'root',
})
export class AuthGuard implements CanActivate {
  constructor(private authService: AuthService, private router: Router) {}

  // eslint-disable-next-line @typescript-eslint/no-unused-vars
  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    if (this.authService.currentUserValue) {
      const userActivities = this.authService.currentUserValue.userActivities;
      // if (route.data['role'] && route.data['role'].filter(function(n: string) {
      //   return userActivities.indexOf(n) !== -1 || n.indexOf("") !== -1;
      // })) {
      // if (route.data['role'] && route.data['role'].filter(function(n: string) {
      //   return userActivities.indexOf(n) === -1 || n.indexOf("") === -1;
      // })) {
      //   this.router.navigate(['/account/login']);
      //   return false;
      // }
      return true;
    }

    this.router.navigate(['/account/login']);
    return false;
  }
}
