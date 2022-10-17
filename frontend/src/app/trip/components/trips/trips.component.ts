import {Component, EventEmitter, OnInit, Output} from '@angular/core';
import {map, Observable, startWith, Subject, takeUntil} from "rxjs";
import {FormBuilder, FormControl, FormGroup, Validators} from "@angular/forms";
import {TripService} from "../../../../../sevices/trip.service";
import {TripModel} from "../../../../models/TripModel";
import {DecimalPipe} from "@angular/common";
import {UserService} from "../../../../../sevices/user.service";

@Component({
  selector: 'app-trips',
  templateUrl: './trips.component.html',
  styleUrls: ['./trips.component.scss']
})
export class TripsComponent implements OnInit {
  trips: TripModel[] = [];
  private readonly unsubscribe$ = new Subject<void>();
  filter = new FormControl('', {nonNullable: true});
  form: FormGroup;

  constructor(private tripService:TripService, private userService:UserService, private fb: FormBuilder ) { }

  ngOnInit(): void {
    this.tripService.getTrips()
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(currentTrips=>{
        this.trips = currentTrips
      });

    this.FilterText()
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(currentUser=>this.trips=currentUser);

    this.form = this.fb.group({
      number: '',
    })
  }

  joinToTrip(){
    let a: number= this.form.get('number').value-1;
    console.log("a ",a);
    let tripId:string =this.trips[a].id;
    this.userService.joinUserToTrip(tripId).subscribe();
  }

  FilterText(): Observable<TripModel[]>{
    return this.filter.valueChanges.pipe(
      startWith(''),
      map(text => {
        return this.trips.filter(trip => {
          const term = text.toLowerCase();
          return (trip.route.startRoute.toLowerCase().includes(term))
            || (trip.route.endRoute.toLowerCase().includes(term));
        });
      })
    );
  }

  ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }

}
