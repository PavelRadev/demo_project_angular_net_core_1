import { Injectable } from '@angular/core';
import {environment} from "../../../../environments/environment";

@Injectable({
  providedIn: 'root'
})
export class AppSettingsService {
  public getAPIUrl(relativePath): string {
    if (relativePath.indexOf('/') === 0) {
      return environment.apiBasePath + relativePath;
    } else {
      return environment.apiBasePath + '/' + relativePath;
    }
  }
}
