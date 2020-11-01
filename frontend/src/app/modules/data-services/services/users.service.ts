import { Observable } from 'rxjs';
import { ApiClientService } from '../../core/services/api-client.service';
import { ListWithTotals } from '../models/ListWithTotals';
import { Injectable } from '@angular/core';
import { User } from '../../core/models/user';
import { ApiResponse } from '../../core/models/api-response';
import {SoftDeletableFilterModel} from "../models/filterParams/soft-deletable-filter.model";

@Injectable()
export class UsersService {
  constructor(private apiClient: ApiClientService) {
  }

  getAllUsersList = (getParams: Partial<SoftDeletableFilterModel> = {}): Observable<ListWithTotals<User>> =>
    this.apiClient.get<ListWithTotals<User>>('users', getParams)

  getById = (id: string): Observable<User> =>
    this.apiClient.get<User>(`users/${id}`)

  deactivateUser = (id: string): Observable<ApiResponse<boolean>> =>
  this.apiClient.post<ApiResponse<boolean>>(`users/deactivate/${id}`)

  CheckEmailIsAlreadyTaken = (email: string): Observable<boolean> =>
    this.apiClient.post<boolean>('auth/is-email-taken', { value: email })
}
