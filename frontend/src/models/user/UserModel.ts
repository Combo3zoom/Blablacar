import {UserTripModel} from "../UserTripModel";

export interface UserModel {
  id: string,
  name: string,
  role: number,
  isVerification: boolean,
  userCreatedAt: string,
  refreshToken: string,
  userTrips: UserTripModel[] | null
}
