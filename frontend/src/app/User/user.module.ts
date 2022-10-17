import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { EditCurrentUserComponent } from './components/edit-current-user/edit-current-user.component';
import {RouterModule, Routes} from "@angular/router";
import {CurrentUserComponent} from "./components/current-user/current-user.component";
import {UserService} from "../../../sevices/user.service";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import { AllUsersComponent } from './components/all-users/all-users.component';
import {TripService} from "../../../sevices/trip.service";

const routes: Routes=[
    {path: "current-user", component:CurrentUserComponent},
    {path: "edit-current-user", component:EditCurrentUserComponent},
    {path: "all-users", component:AllUsersComponent},
]

@NgModule({
  declarations: [
    CurrentUserComponent,
    AllUsersComponent,
    EditCurrentUserComponent
  ],
  imports: [
    CommonModule,
    RouterModule.forChild(routes),
    FormsModule,
    ReactiveFormsModule,
  ],
  exports: [EditCurrentUserComponent],
  providers: [UserService, TripService],
  bootstrap:[]
})
export class UserModule { }
