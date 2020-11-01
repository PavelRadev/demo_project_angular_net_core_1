import {ApiErrorDetailsModel} from "./api-error-details";

export class ApiResponse<T> {
  public status: string;
  public data: T;
  public errorDetails: ApiErrorDetailsModel;
}
