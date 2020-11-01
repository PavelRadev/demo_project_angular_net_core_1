import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { ListWithTotals } from '../models/ListWithTotals';
import { BaseListFilterModel } from '../models/filterParams/base-list-filter-model';
import { User } from '../../core/models/user';

@Injectable()
export class UsersServiceMock {

  public getAllUsersList(getParams: Partial<BaseListFilterModel> = {}):  Observable<ListWithTotals<User>> {

    var listOfPerson : User[] = [
      {
        firstName: 'test1',
        lastName: 'test1',
        companyName: 'testCompany',
        email: 'test1@mail.com',
        id: "1"
      },{
        firstName: 'test2',
        lastName: 'test2',
        companyName: 'test2Company',
        email: 'test2@mail.com',
        id: "2"
      },{
        firstName: 'test3',
        lastName: 'test3',
        companyName: 'test3Company',
        email: 'test3@mail.com',
        id: "3"
      },
    ];

    var listWithTotals = new ListWithTotals<User>();
    listWithTotals.totalCount = 3;
    listWithTotals.currentPage = 0;
    listWithTotals.pageSize = 5;
    listWithTotals.skippedItems = 0;
    listWithTotals.list = listOfPerson;

    return of(listWithTotals)
  }

}
