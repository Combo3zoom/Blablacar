import {Injectable} from "@angular/core";
import {map, Observable} from "rxjs";
import {UserModel} from "../src/models/user/UserModel";
import {environment} from "../src/environments/environment";
import {HttpClient} from "@angular/common/http";
import {DatePipe} from "@angular/common";
import {TripModel} from "../src/models/TripModel";

@Injectable()
export class TripService{
  constructor(private http: HttpClient, private datePipe: DatePipe) {
  }

  public getTrips():Observable<TripModel[]>{
    const url = `${environment.apiUrl}/Trip`;
    return this.http.get<TripModel[]>(url).pipe(map((trips:TripModel[])=>{
      for (let i=0; i<trips.length; i++ ){
        trips[i].tripCreatedAt = this.datePipe.transform(trips[i].tripCreatedAt, 'MMM d, y, h:mm a');
        trips[i].departureAt = this.datePipe.transform(trips[i].departureAt, 'MMM d, y, h:mm a');
      }
      return trips;
    }));
  };

  public getTripById(id:string): Observable<TripModel>{
    const url = `${environment.apiUrl}/User/${id}`;
    return this.http.get<TripModel>(url).pipe(map((trip:TripModel)=>{
      trip.tripCreatedAt = this.datePipe.transform(trip.tripCreatedAt, 'MMM d, y, h:mm a');
      trip.departureAt = this.datePipe.transform(trip.departureAt, 'MMM d, y, h:mm a');
      return trip;
    }));
  };
}


