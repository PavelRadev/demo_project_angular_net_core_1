import {Component, OnInit, ViewChild} from '@angular/core';
import {BaseSafeSubscriber} from "../../../shared/components-base/base-safe-subscriber";
import {NgForm} from "@angular/forms";
import {SessionService} from "../../../core/services/session.service";
import {Router} from "@angular/router";
import {FormsUtils} from "../../../shared/utils/forms-utils";

@Component({
  selector: 'app-signin',
  templateUrl: './signin.component.html'
})
export class SigninComponent extends BaseSafeSubscriber implements OnInit {
  @ViewChild('loginForm') loginForm: NgForm;

  public login = 'main@mailinator.com';
  public password = 'Main1718';

  error: string;

  public isLoading: boolean;
  public isPasswordVisible = false;

  constructor(private sessionService: SessionService,
              private router: Router) {
    super();
  }

  ngOnInit(): void {
    this.registerSubscription(this.sessionService.CurrentUserObservable
      .subscribe((user) => {
        if (user) {
          this.router.navigate(['/']).then();
        }
      }));
  }

  async signin(): Promise<void> {
    this.isLoading = true;

    try {
      const authResult = await this.sessionService.SigninWithCredentials(this.login, this.password);
      this.error = null;
      this.isLoading = false;
    } catch (e) {
      this.error = e.message || null;

      FormsUtils.TryToApplyBackendErrorsToForm(this.loginForm, e);
    } finally {
      this.isLoading = false;
    }
  }

  public getPasswordInputType(): string {
    return this.isPasswordVisible ? 'text' : 'password';
  }

  public switchPasswordInputType(): boolean {
    this.isPasswordVisible = !this.isPasswordVisible;

    return this.isPasswordVisible;
  }
}
