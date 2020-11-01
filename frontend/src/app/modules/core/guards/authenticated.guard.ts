import {Injectable} from '@angular/core';
import {ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree} from '@angular/router';
import {Observable} from 'rxjs';
import {first} from "rxjs/operators";
import {SessionService} from "../services/session.service";

@Injectable({
  providedIn: 'root'
})
export class AuthenticatedGuard implements CanActivate {
  constructor(private sessionService: SessionService,
              private router: Router) {
  }

  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    return new Promise((resolve) => {
      this.sessionService.CurrentUserObservable.pipe(first()).subscribe(user => {
        if (user) {
          resolve(true);
        } else {
          this.router.navigate(['/auth/login']);
          resolve(false);
        }
      });
    });
  }
}
