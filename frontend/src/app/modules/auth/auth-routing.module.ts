import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import {SigninComponent} from "./components/signin/signin.component";
import {SignupComponent} from "./components/signup/signup.component";

const routes: Routes = [
  {
    path: '',
    children: [
      {
        path: 'login',
        component: SigninComponent,
        data: {title: 'Sign In'}
      },
      {
        path: 'signup',
        component: SignupComponent,
        data: {title: 'Registration'}
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AuthRoutingModule { }
