import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {LoadingAnimationComponent} from './components/loading-animation/loading-animation.component';
import {BaseAsyncValidatorDirective} from "./directives/base-async-validator.directive";


@NgModule({
  declarations: [
    LoadingAnimationComponent,
    BaseAsyncValidatorDirective
  ],
  exports: [
    LoadingAnimationComponent,
    BaseAsyncValidatorDirective
  ],
  imports: [
    CommonModule
  ]
})
export class SharedModule {
}
