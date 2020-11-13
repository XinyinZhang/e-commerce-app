import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { AccountService } from 'src/app/account/account.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  // accountService: to check if the user is currently logged in
  // router: redirect the user
  constructor(private accountService: AccountService, private router: Router) {}

  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean> {
      // check if user is logged in
    return this.accountService.currentUser$.pipe(
      map(auth => {
        if (auth) { // if user is currently logged in
          return true;
        }
        // user is not log in
        this.router.navigate(['account/login'], {queryParams: {returnUrl: state.url}});
      })
    );
  }

}
