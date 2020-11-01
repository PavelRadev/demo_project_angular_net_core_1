import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { CoreModule } from "./modules/core/core.module";
import { ToastrModule } from "ngx-toastr";
import { DataServicesModule } from './modules/data-services/data-services.module';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    CoreModule.forRoot(),
    ToastrModule.forRoot({
      progressAnimation: 'decreasing',
      preventDuplicates: true,
      countDuplicates: true,
      resetTimeoutOnDuplicate: true,
      enableHtml: true
    }),
    DataServicesModule.forRoot(),
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
