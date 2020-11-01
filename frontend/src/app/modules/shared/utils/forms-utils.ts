import {NgForm} from '@angular/forms';
import {ApiErrorDetailsModel} from "../../core/models/api-error-details";
import forEach from 'lodash-es/forEach';

export class FormsUtils {
  public static TryToApplyBackendErrorsToForm(form: NgForm, errorData: ApiErrorDetailsModel): void {
    forEach(errorData.fieldSpecificMessages, (v, k) => {
      const controlToApplyChangesTo = form.controls[k];

      if (controlToApplyChangesTo) {
        controlToApplyChangesTo.setErrors({serverError: v});
      }
    });
  }
}
