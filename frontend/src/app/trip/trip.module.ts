import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {RouterModule, Routes} from "@angular/router";
import {CurrentUserTripsComponent} from "./components/current-user-trips/current-user-trips.component";
import {EditCurrentUserTripComponent} from "./components/edit-current-user-trip/edit-current-user-trip.component";
import {TripsComponent} from "./components/trips/trips.component";
import {TripService} from "../../../sevices/trip.service";
import {ReactiveFormsModule} from "@angular/forms";
import {UserService} from "../../../sevices/user.service";


const routes: Routes=[
  {path:"current-user-trips", component:CurrentUserTripsComponent},
  {path:"edit-current-user-trip", component:EditCurrentUserTripComponent},
  {path:"trips", component:TripsComponent},
]

@NgModule({
  declarations: [CurrentUserTripsComponent, EditCurrentUserTripComponent, TripsComponent],
  imports: [
    CommonModule,
    RouterModule.forChild(routes),
    ReactiveFormsModule,
  ],
  providers:[TripService, UserService],
  bootstrap:[]
})
export class TripModule { }
