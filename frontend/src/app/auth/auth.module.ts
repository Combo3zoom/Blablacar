import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {RouterModule, Routes} from "@angular/router";
import {ReactiveFormsModule} from "@angular/forms";

import {RegisterComponent} from "./components/register/register.component";
import {AuthService} from "../../../sevices/auth/auth.service";
import { LoginComponent } from './components/login/login.component';
import {StoreModule} from "@ngrx/store";

const routes:Routes =[
  { path: 'register', component: RegisterComponent},
  { path: 'login', component: LoginComponent},
]

@NgModule({
  declarations: [RegisterComponent, LoginComponent],
  imports: [
    CommonModule,
    RouterModule.forChild(routes),
    ReactiveFormsModule,
  ],
  providers: [AuthService]
})
export class AuthModule { }
