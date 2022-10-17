import {UserModel} from "./user/UserModel";
import {UserTripModel} from "./UserTripModel";

export interface TripModel {
  id: string,
  routeId: string,
  route: RouteModel,
  departureAt: string,
  tripCreatedAt: string,
  userTrips: UserTripModel[] | null
}

export interface RouteModel {
  id:string,
  startRoute:string,
  endRoute: string,
  trips: TripModel[] | null
}
