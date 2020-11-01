import {Component, OnInit, ViewChild} from '@angular/core';
import {NgForm} from '@angular/forms';
import {Router} from '@angular/router';
import {SessionService} from 'src/app/modules/core/services/session.service';
import {UserRegistrModel} from 'src/app/modules/data-services/models/requests/user-registr.model';
import {BaseSafeSubscriber} from 'src/app/modules/shared/components-base/base-safe-subscriber';
import {FormsUtils} from 'src/app/modules/shared/utils/forms-utils';
import {UsersService} from "../../../data-services/services/users.service";
import {RegExpUtils} from "../../../core/utils/RegExpUtils";

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html'
})
export class SignupComponent extends BaseSafeSubscriber implements OnInit {

  @ViewChild('registerForm') loginForm: NgForm;
  public emailPattern: RegExp = RegExpUtils.GetEmailRegExp();
  public passwordPattern: RegExp = RegExpUtils.GetPasswordRegExp();

  public model = new UserRegistrModel();

  public isTersmChecked = false;
  public isPasswordVisible = false;
  public isConfirmPasswordVisible = false;

  error: string;

  public isLoading: boolean;

  constructor(private sessionService: SessionService,
              private usersService: UsersService,
              private router: Router) {
    super();
  }

  ngOnInit() {
    this.registerSubscription(this.sessionService.CurrentUserObservable
      .subscribe((user) => {
        if (user) {
          this.router.navigate(['/']).then();
        }
      }));
  }

  async signup(): Promise<void> {
    this.isLoading = true;

    try {
      const result = await this.sessionService.RegistrUser(this.model);
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

  public getConfirmPasswordInputType(): string {
    return this.isConfirmPasswordVisible ? 'text' : 'password';
  }

  uniqEmailValidatorFunc = async (): Promise<any> => {
    try {
      const response = await this.usersService.CheckEmailIsAlreadyTaken(this.model.email).toPromise();

      return response ? true : null;
    }
    catch (e) {
      return `Error occured ${e}`;
    }
  }
}
