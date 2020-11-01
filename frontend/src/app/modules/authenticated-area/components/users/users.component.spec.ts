import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { ToastrModule } from 'ngx-toastr';
import { UsersService } from '../../../data-services/services/users.service';
import { UsersServiceMock } from '../../../data-services/services/users.service.mock';
import { UsersComponent } from './users.component';
import { ApiClientService } from '../../../core/services/api-client.service';
import { SessionService } from '../../../core/services/session.service';
import { LocalStorageExtendedService } from '../../../core/services/local-storage-extended.service';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { PageEvent } from '@angular/material/paginator';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

describe('UsersComponent', () => {
  let component: UsersComponent;
  let fixture: ComponentFixture<UsersComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ UsersComponent ],
      imports: [
        HttpClientTestingModule, 
        ToastrModule.forRoot(),
        MatTableModule,
        MatPaginatorModule,
        BrowserAnimationsModule,
        RouterTestingModule.withRoutes( [{
          path: 'users',
          component: UsersComponent
        }])
      ],
      providers: [ 
        { provide: UsersService, useClass: UsersServiceMock },
        ApiClientService,
        SessionService,
        LocalStorageExtendedService
      ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UsersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should have initial values', () => {
    expect(component.isLoading).toBe(true);
    expect(component.filters).not.toBeNull();
  });

  describe('#updateData', () => {
    it('should fill tableData', async function () {
      await component.updateData();

      expect(component.tableData.list.length).toBeGreaterThan(0);
      expect(component.tableData.isEmpty).toBe(false);
      expect(component.isAnyClientsExists).toBe(true);
    });
  });

  describe('#onPaginationChanged', () => {
    it('should change filters state', () => {
      const event: PageEvent = {
        pageSize: 20,
        pageIndex: 0,
        length: 100
      };

      component.onPaginationChanged(event);

      expect(component.filters.pageSize).toBe(event.pageSize);
      expect(component.filters.page).toBe(event.pageIndex);
    });
  });

  
});

