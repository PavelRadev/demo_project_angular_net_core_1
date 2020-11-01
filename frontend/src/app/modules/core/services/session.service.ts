import {Injectable} from '@angular/core';
import {Observable, ReplaySubject} from "rxjs";
import {HttpClient} from "@angular/common/http";
import {LocalStorageExtendedService} from "./local-storage-extended.service";
import {environment} from "../../../../environments/environment";
import {User} from "../models/user";
import {AuthResult} from "../models/auth-result";
import {ApiResponse} from "../models/api-response";
import {AppSettingsService} from "./app-settings.service";
import {map} from "rxjs/operators";
import {UserRegistrModel} from '../../data-services/models/requests/user-registr.model';

@Injectable({
  providedIn: 'root'
})
export class SessionService {
  private token: string;
  private tokenSubject = new ReplaySubject<string>(1);
  private currentUser: User;
  private currentUserSubject = new ReplaySubject<User>(1);

  constructor(private httpClient: HttpClient,
              private localStorage: LocalStorageExtendedService,
              private appSettingsService: AppSettingsService) {
    this.reinitializeTokenFromLocalStorage();
    this.ReinitializeCurrentUser().then();

    this.localStorage.OnStorageChange$.subscribe((x) => {
      if (x.key === environment.currentUserLocalStorageKey) {
        this.setCurrentUser(x.value);
      }

      if (x.key === environment.JWTTokenLocalStorageKey) {
        this.setToken(x.value);
      }
    });
  }

  public get Token(): string {
    return this.token;
  }

  public get TokenObservable(): Observable<string> {
    return this.tokenSubject.asObservable();
  }

  public get CurrentUser(): User {
    return this.currentUser;
  }

  public get CurrentUserObservable(): Observable<User> {
    return this.currentUserSubject.asObservable();
  }

  public setToken(newToken: string, setLocalStorage: boolean = false): void {
    if (setLocalStorage) {
      this.localStorage.Set(environment.JWTTokenLocalStorageKey, newToken);
    }

    this.token = newToken;
    this.tokenSubject.next(this.token);
  }

  public PerformLogout(): void {
    this.localStorage.Remove(environment.JWTTokenLocalStorageKey);
    this.localStorage.Remove(environment.currentUserLocalStorageKey);
    this.setToken(null, true);
    this.setCurrentUser(null, true);
  }

  public async SigninWithCredentials(login: string, password: string): Promise<AuthResult> {
    try {
      const authResponse = await this.httpClient
        .post<ApiResponse<AuthResult>>(this.appSettingsService.getAPIUrl('/auth/signin-with-credentials'), {
          login,
          password
        })
        .toPromise();


      if (authResponse.errorDetails) {
        throw authResponse.errorDetails;
      }

      this.setToken(authResponse.data.token, true);
      await this.ReinitializeCurrentUser();
      return authResponse.data;
    } catch (e) {
      this.PerformLogout();
      throw e.error.errorDetails || e;
    }
  }

  public async RegistrUser(model: UserRegistrModel): Promise<AuthResult> {
    try {
      const authResponse = await this.httpClient
        .post<ApiResponse<AuthResult>>(this.appSettingsService.getAPIUrl('/auth/register'), model)
        .toPromise();

      if (authResponse.errorDetails) {
        throw authResponse.errorDetails;
      }

      this.setToken(authResponse.data.token, true);
      await this.ReinitializeCurrentUser();
      return authResponse.data;
    } catch (e) {
      this.PerformLogout();
      throw e.error.errorDetails || e;
    }
  }

  public async ReinitializeCurrentUser(): Promise<User> {
    if (!this.Token) {
      console.log('empty this.Token', this.Token);
      this.PerformLogout();
      return null;
    }

    try {
      const currentUserResponse = await this.httpClient
        .get<ApiResponse<User>>(this.appSettingsService.getAPIUrl('/auth/current-user'))
        .pipe(
          map((value) => {
            return value;
          })
        )
        .toPromise();

      this.setCurrentUser(currentUserResponse.data, true);
    } catch (e) {
      console.log('e', e);
      this.PerformLogout();
    }
  }

  private setCurrentUser(user: User, setLocalStorage: boolean = false): void {
    if (setLocalStorage) {
      this.localStorage.Set(environment.currentUserLocalStorageKey, user);
    }

    this.currentUser = user;
    this.currentUserSubject.next(this.currentUser);
  }

  private reinitializeTokenFromLocalStorage(): void {
    const tokenFromLocalStorage = this.localStorage.Get(environment.JWTTokenLocalStorageKey);
    this.setToken(tokenFromLocalStorage);
  }
}
