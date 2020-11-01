﻿import { Directive, forwardRef, Input } from '@angular/core';
import { AbstractControl, AsyncValidator, NG_ASYNC_VALIDATORS, ValidationErrors } from '@angular/forms';

@Directive({
  selector: '[appBaseAsyncValidator]',
  providers: [
    {
      provide: NG_ASYNC_VALIDATORS,
      useExisting: forwardRef(() => BaseAsyncValidatorDirective), multi: true
    },
  ]
})
export class BaseAsyncValidatorDirective implements AsyncValidator {
  @Input() validationFunctionToUse: (value: string) => Promise<any>;
  @Input() validationErrorName: string;
  @Input() validationDelay = 400;

  private validationTimeout: number;

  async validate(control: AbstractControl): Promise<ValidationErrors | null> {
    clearTimeout(this.validationTimeout);
    return new Promise((resolve, reject) => {
      this.validationTimeout = setTimeout(async () => {
        this.validationFunctionToUse(control.value)
          .then((res) => {
            resolve(res ? { [this.validationErrorName]: res } : null);
          })
          .catch((err) => {
            reject(err);
          });
      }, this.validationDelay);
    });
  }
}
