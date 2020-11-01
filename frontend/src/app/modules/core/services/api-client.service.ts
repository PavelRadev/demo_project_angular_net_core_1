import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpEvent, HttpEventType, HttpResponse } from '@angular/common/http';
import { SessionService } from './session.service';
import { AppSettingsService } from './app-settings.service';
import { catchError, map } from 'rxjs/operators';
import reduce from 'lodash-es/reduce';
import chain from 'lodash-es/chain';
import { Observable, throwError } from 'rxjs';
import { ApiResponse } from '../models/api-response';
import { ApiErrorDetailsModel } from '../models/api-error-details';
import has from 'lodash-es/has';

@Injectable()
export class ApiClientService {

  constructor(private httpClient: HttpClient, private sessionService: SessionService, private appSettingsService: AppSettingsService) {
  }

  get<T>(url: string, getParams: any = {}): Observable<T> {
    const transformedParams = reduce(getParams, (acc, v, k) => {
      if (v instanceof Date) {
        acc[k] = v.toUTCString();
      } else {
        acc[k] = v;
      }

      return acc;
    }, {});

    return this.httpClient
      .get<ApiResponse<T>>(this.appSettingsService.getAPIUrl(url), {
        params: transformedParams,
        observe: 'body',
        responseType: 'json'
      })
      .pipe(
        map((response) => this.handleApiResponse<T>(response)),

        //catchError((response) => this.handleApiError(response))
      );
  }

  post<T>(url: string, body: any = {}, getParams: any = {}): Observable<T> {
    return this.httpClient
      .post<ApiResponse<T>>(this.appSettingsService.getAPIUrl(url), body, {
        params: getParams,
        observe: 'body',
        responseType: 'json'
      })
      .pipe(
        map((response) => this.handleApiResponse<T>(response)),

        //catchError((response) => this.handleApiError(response))
      );
  }

  put<T>(url: string, body: any = {}, getParams: any = {}): Observable<T> {
    return this.httpClient
      .put<ApiResponse<T>>(this.appSettingsService.getAPIUrl(url), body, {
        params: getParams,
        observe: 'body',
        responseType: 'json'
      })
      .pipe(
        map((response) => this.handleApiResponse<T>(response)),

        //catchError((response) => this.handleApiError(response))
      );
  }

  delete<T>(url: string, getParams: any = {}): Observable<T> {
    return this.httpClient
      .delete<ApiResponse<T>>(this.appSettingsService.getAPIUrl(url), { params: getParams, observe: 'body', responseType: 'json' })
      .pipe(
        map((response) => this.handleApiResponse<T>(response)),

        //catchError((response) => this.handleApiError(response))
      );
  }

  private createRequestOptions(getParams: any = {}): any {
    return { params: getParams, observe: 'body', responseType: 'json' };
  }

  private handleApiResponse<T>(response: ApiResponse<T>): T {
    if (!response) {
      throw new Error('Empty response');
    }

    if (response.errorDetails) {
      throw response.errorDetails;
    }

    return response.data;
  }

  /*private handleApiError(response: HttpEvent<any> | any): Observable<never> {
    let detailedError: ApiErrorDetailsModel;
    const error = !!response.error ? response.error : response;
    console.log('error', error);

    if (!(response instanceof HttpErrorResponse)) {
      const errMessage = !!response.message ? response.message : response.toString();
      detailedError = this.tryParseErrorOrSetDefaultMessage(error, errMessage);
    }

    if (response instanceof HttpErrorResponse) {
      if (response.status === 500) {
        detailedError = this.tryParseErrorOrSetDefaultMessage(error, 'Server error');
      }

      if (response.status === 401) {
        this.sessionService.PerformLogout();

        detailedError = this.tryParseErrorOrSetDefaultMessage(error, 'Unauthorized');
      }

      if (response.status === 403) {
        detailedError = this.tryParseErrorOrSetDefaultMessage(error, 'Access denied');
      }

      if (response.status === 404) {
        detailedError = this.tryParseErrorOrSetDefaultMessage(error, 'Route you are trying to request isn\'t exist');
      }

      if (response.status === 400) {
        const title = response.error.title;
        const errorsList = response.error.errors;

        if (title && errorsList.length) {
          const processedErrorsList = chain(errorsList)
            .map((x) => `"${x}"`)
            .join(', ')
            .value();

          detailedError = this.tryParseErrorOrSetDefaultMessage(error, `${title}: ${processedErrorsList}`);
        } else {
          detailedError = this.tryParseErrorOrSetDefaultMessage(error, 'Validation Error');
        }
      }

      if (!detailedError) {
        detailedError = this.tryParseErrorOrSetDefaultMessage(error, 'Server error');
      }
    }

    console.log(detailedError, 'color:blue;');
    return throwError(detailedError);
  }

  /*private tryParseErrorOrSetDefaultMessage(error: any, defaultMessage: string): ApiErrorDetailsModel {
    const res = new ApiErrorDetailsModel();

    if (!has(error, 'errorDetails')) {
      res.message = defaultMessage;

      return res;
    }

    res.fieldSpecificMessages = error?.errorDetails?.fieldSpecificMessages;
    res.message = error?.errorDetails?.message;

    return res;
  }*/
}
