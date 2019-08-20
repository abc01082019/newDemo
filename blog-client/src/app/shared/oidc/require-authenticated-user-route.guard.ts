import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { OpenIdConnectService } from './open-id-connect.service';

@Injectable({
  providedIn: 'root'
})

/**
 *  Set permissions of file/resource, that require user authentication
 */
export class RequireAuthenticatedUserRouteGuard implements CanActivate {
  constructor(
    private openIdConnectService: OpenIdConnectService,
    private router: Router) { }

  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean> | Promise<boolean> | boolean {
    // check if the user is logged in 
    if (this.openIdConnectService.userAvailable) {
      return true;
    } else {
      // trigger signin
      this.openIdConnectService.triggerSignIn();
      return false;
    }
  }
}
