import { ModuleWithProviders, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UsersService } from './services/users.service';


@NgModule({
  declarations: [],
  imports: [
    CommonModule
  ]
})
export class DataServicesModule {
  static forRoot(): ModuleWithProviders<DataServicesModule> {
    return {
      ngModule: DataServicesModule,
      providers: [
        UsersService
      ]
    };
  }
}
