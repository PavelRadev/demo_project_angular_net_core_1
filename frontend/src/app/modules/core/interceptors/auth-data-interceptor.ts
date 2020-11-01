﻿import {HttpEvent, HttpHandler, HttpInterceptor, HttpRequest} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {environment} from '../../../../environments/environment';
import {Observable} from 'rxjs';
import {LocalStorageExtendedService} from '../services/local-storage-extended.service';

@Injectable()
export class AuthDataInterceptor implements HttpInterceptor {
  constructor(private localStorage: LocalStorageExtendedService) {
  }

  intercept(
    req: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    const accessToken = this.localStorage.Get(environment.JWTTokenLocalStorageKey);

    let requestClone = req.clone();
    if (accessToken) {
      requestClone = requestClone.clone({
        setHeaders: {
          Authorization: `Bearer ${accessToken}`,
        }
      });
    }

    return next.handle(requestClone);
  }
}
