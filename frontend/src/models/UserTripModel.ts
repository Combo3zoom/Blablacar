import {UserModel} from "./user/UserModel";
import {TripModel} from "./TripModel";

export interface UserTripModel {
  user: UserModel | null,
  userId: string | null,
  trip: TripModel | null,
  tripId: string | null
}
