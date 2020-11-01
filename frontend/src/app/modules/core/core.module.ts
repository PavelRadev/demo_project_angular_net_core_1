import {ModuleWithProviders, NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {LocalStorageExtendedService} from "./services/local-storage-extended.service";
import {AppSettingsService} from "./services/app-settings.service";
import {SessionService} from "./services/session.service";
import {HTTP_INTERCEPTORS, HttpClientModule} from "@angular/common/http";
import {AuthDataInterceptor} from "./interceptors/auth-data-interceptor";
import { ApiClientService } from './services/api-client.service';

@NgModule({
  imports: [
    CommonModule,
    HttpClientModule
  ],
})
export class CoreModule {
  static forRoot(): ModuleWithProviders<CoreModule> {
    return {
      ngModule: CoreModule,
      providers: [
        LocalStorageExtendedService,
        {
          provide: HTTP_INTERCEPTORS,
          useClass: AuthDataInterceptor,
          multi: true,
        },
        SessionService,
        ApiClientService,
      ]
    };
  }
}
