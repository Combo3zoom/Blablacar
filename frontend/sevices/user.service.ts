import {Injectable} from "@angular/core";
import {map,Observable} from "rxjs";
import {UserModel} from "../src/models/user/UserModel";
import {environment} from "../src/environments/environment";
import {HttpClient} from "@angular/common/http";
import {DatePipe} from "@angular/common";
import {UpdateUserModel} from "../src/models/user/updateUserModel";
import {TripService} from "./trip.service";
import {TripModel} from "../src/models/TripModel";

@Injectable()
export class UserService{

  constructor(private http: HttpClient, private datePipe: DatePipe, private tripService:TripService) {
  }

  public getUsers():Observable<UserModel[]>{
    const url = `${environment.apiUrl}/User`;
    return this.http.get<UserModel[]>(url).pipe(map((users:UserModel[])=>{
      for (let i=0; i<users.length; i++ ){
        users[i].userCreatedAt = this.datePipe.transform(users[i].userCreatedAt, 'MMM d, y, h:mm a')
      }
      return users;
    }));
  };

  public getCurrentUser(): Observable<UserModel>{
    const url = `${environment.apiUrl}/me`;
    return this.http.get<UserModel>(url).pipe(map((user:UserModel)=>{
      user.userCreatedAt = this.datePipe.transform(user.userCreatedAt, 'MMM d, y, h:mm a');
      return user;
    }));
  };

  public getUserById(id:string): Observable<UserModel>{
    const url = `${environment.apiUrl}/User/${id}`;
    return this.http.get<UserModel>(url).pipe(map((user:UserModel)=>{
      user.userCreatedAt = this.datePipe.transform(user.userCreatedAt, 'MMM d, y, h:mm a')
      return user;
    }));
  };

  public updateUser(user: UpdateUserModel): Observable<UserModel>{
    const url = `${environment.apiUrl}/User/me/put`
    return this.http.put<UserModel>(url,user);
  };

  public getUserTrips():Observable<TripModel[]>{
    const url = `${environment.apiUrl}/me/trips`;
    return this.http.get<TripModel[]>(url).pipe(map((trips:TripModel[])=>{
      for (let i=0; i<trips.length; i++ ){
        trips[i].tripCreatedAt = this.datePipe.transform(trips[i].tripCreatedAt, 'MMM d, y, h:mm a');
        trips[i].departureAt = this.datePipe.transform(trips[i].departureAt, 'MMM d, y, h:mm a');
      }
      return trips;
    }));
  };

  public joinUserToTrip(tripId: string):Observable<Object>{
    const url = `${environment.apiUrl}/User/me/trips/${tripId}/joinToTrip`;
    return this.http.post(url, {});
  }
}
