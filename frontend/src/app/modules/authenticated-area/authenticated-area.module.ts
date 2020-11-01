import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatSortModule } from '@angular/material/sort';
import { AuthenticatedAreaRoutingModule } from './authenticated-area-routing.module';
import { MainLayoutComponent } from './components/main-layout/main-layout.component';
import { UsersComponent } from './components/users/users.component';
import { NotFoundComponent } from './components/not-found/not-found.component';
import { AppHeaderComponent } from './components/app.header/app.header.component';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from "@angular/material/button";
import { MatTableModule } from '@angular/material/table';
import { MatMenuModule } from '@angular/material/menu';
import { MatIconModule } from '@angular/material/icon';
import { MatPaginatorModule } from '@angular/material/paginator';

@NgModule({
  declarations: [
    MainLayoutComponent,
    UsersComponent,
    NotFoundComponent,
    AppHeaderComponent
  ],
  imports: [
    CommonModule,
    AuthenticatedAreaRoutingModule,
    MatToolbarModule,
    MatButtonModule,
    MatSortModule,
    MatTableModule,
    MatMenuModule,
    MatIconModule,
    MatPaginatorModule
  ]
})
export class AuthenticatedAreaModule { }
