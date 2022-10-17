import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import {CurrentUserComponent} from "./User/components/current-user/current-user.component";
import {TripsComponent} from "./trip/components/trips/trips.component";

const routes: Routes=[
  {path: 'current-user', component: CurrentUserComponent},
  {path: 'trips', component: TripsComponent},
  {path: '', pathMatch: "full", redirectTo: 'all-users'},
]

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
