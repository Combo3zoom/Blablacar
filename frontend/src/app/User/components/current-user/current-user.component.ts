import {Component, Input, OnInit} from '@angular/core';
import {UserModel} from "../../../../models/user/UserModel";
import {UserService} from "../../../../../sevices/user.service";
import {Roles} from "../../../../../enums/roles";
import {Subject, takeUntil} from "rxjs";
import {TripModel} from "../../../../models/TripModel";


@Component({
  selector: 'app-current-user',
  templateUrl: './current-user.component.html',
  styleUrls: ['./current-user.component.scss']
})
export class CurrentUserComponent implements OnInit {
  user: UserModel={
    id: '',
    name: '',
    role: Roles.BaseUser,
    isVerification: false,
    userCreatedAt: '',
    refreshToken: '',
    userTrips: null
  };
  private readonly unsubscribe$ = new Subject<void>();
  userToEdit: UserModel;
  isEditIcon: Boolean;
  trips: TripModel[];

  constructor(private userService: UserService) { }

  ngOnInit(): void {
    this.isEditIcon = false;
    this.userService.getCurrentUser()
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(user=>{
      this.user= user;
    });

    this.userService.getUserTrips()
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe((currentTrips:TripModel[])=>this.trips=currentTrips);


  }

  editUser(currentUser: UserModel) {
    this.isEditIcon = !this.isEditIcon;
    this.userToEdit = currentUser;
  }



  ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }

}
