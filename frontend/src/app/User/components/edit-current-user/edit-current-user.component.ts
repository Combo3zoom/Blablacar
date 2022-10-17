import {Component, ElementRef, EventEmitter, Input, OnInit, Output, ViewChild} from '@angular/core';
import {UserModel} from "../../../../models/user/UserModel";
import {UserService} from "../../../../../sevices/user.service";
import {UpdateUserModel} from "../../../../models/user/updateUserModel";
import {Subject, takeUntil} from "rxjs";

@Component({
  selector: 'app-edit-current-user',
  templateUrl: './edit-current-user.component.html',
  styleUrls: ['./edit-current-user.component.scss']
})
export class EditCurrentUserComponent implements OnInit {
  @Input() user? : UserModel;
  @Output() usersUpdated = new EventEmitter<UserModel>();
  private readonly unsubscribe$ = new Subject<void>;

  constructor(private userService: UserService) { }

  ngOnInit(): void {
  }

  updateUser(user: UpdateUserModel) {
    this.userService
      .updateUser(user)
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe((users:UserModel)=>this.usersUpdated.emit(users))
  }

  ngOnDestroy(): void{
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }
}
