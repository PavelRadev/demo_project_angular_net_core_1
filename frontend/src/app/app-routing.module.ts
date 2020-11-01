import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import {AuthenticatedGuard} from "./modules/core/guards/authenticated.guard";

const routes: Routes = [
  {
    path: 'auth',
    loadChildren: './modules/auth/auth.module#AuthModule',
    data: { title: 'Authorization', htmlTitle: 'Authorization' }
  },
  {
    path: '',
    loadChildren: './modules/authenticated-area/authenticated-area.module#AuthenticatedAreaModule',
    canActivate: [ AuthenticatedGuard ]
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
