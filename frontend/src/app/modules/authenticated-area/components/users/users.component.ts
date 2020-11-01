import { Component, OnDestroy, OnInit } from '@angular/core';
import { ListWithTotals } from '../../../data-services/models/ListWithTotals';
import { Subject } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { debounceTime, takeWhile } from 'rxjs/operators';
import { PageEvent } from '@angular/material/paginator';
import { Sort } from '@angular/material/sort';
import isEqual from 'lodash-es/isEqual';
import { User } from 'src/app/modules/core/models/user';
import { BaseSafeSubscriber } from 'src/app/modules/shared/components-base/base-safe-subscriber';
import { UsersService } from '../../../data-services/services/users.service';
import {SoftDeletableFilterModel} from "../../../data-services/models/filterParams/soft-deletable-filter.model";

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html'
})
export class UsersComponent extends BaseSafeSubscriber implements OnInit {

  public tableData: ListWithTotals<User>;
  public isLoading = true;

  public filters = new SoftDeletableFilterModel();
  private updateDataCalledSubject$ = new Subject();

  public isAnyClientsExists = false;
  constructor(private usersService: UsersService,
    private toastr: ToastrService) {
    super();
   }

  ngOnInit(): void {
    this.registerSubscription(
      this.updateDataCalledSubject$
      .pipe(
        debounceTime(300)
      )
      .subscribe(async () => {
        await this.updateData();
      }));


    this.callDataUpdate();
  }

  public async updateData(): Promise<void> {
    try {
      this.isLoading = true;
      this.tableData = await this.usersService.getAllUsersList(this.filters).toPromise();

      if (!this.isAnyClientsExists && this.tableData.totalCount) {
        this.isAnyClientsExists = true;
      }
    } catch (e) {
      console.log('e', e);
      this.toastr.error(e.message || e);
    } finally {
      this.isLoading = false;
    }
  }

  public callDataUpdate(): void {
    this.updateDataCalledSubject$.next();
  }

  onPaginationChanged(event: PageEvent): void {
    this.filters.pageSize = event.pageSize;
    this.filters.page = event.pageIndex;
    this.callDataUpdate();
  }

  onSort(event: Sort): void {
    this.filters.orderedBy = event.active;

    this.filters.orderReversed = isEqual(event.direction, 'asc');
    this.callDataUpdate();
  }

}
