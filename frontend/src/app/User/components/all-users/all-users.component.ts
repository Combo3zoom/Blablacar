import {Component, Input, OnInit, PipeTransform} from '@angular/core';
import {UserService} from "../../../../../sevices/user.service";
import {UserModel} from "../../../../models/user/UserModel";
import {map, Observable, startWith, Subject, takeUntil} from "rxjs";
import {FormControl} from "@angular/forms";
import {DatePipe, DecimalPipe} from "@angular/common";
import {Roles} from "../../../../../enums/roles";



@Component({
  selector: 'app-all-users',
  templateUrl: './all-users.component.html',
  styleUrls: ['./all-users.component.scss']
})
export class AllUsersComponent implements OnInit {
  users: UserModel[] = [];
  private readonly unsubscribe$ = new Subject<void>();
  filter = new FormControl('', {nonNullable: true});

  constructor(private userService:UserService) { }


  ngOnInit(): void {
    this.userService.getUsers()
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(currentUsers=>{
      this.users = currentUsers
    });

    this.FilterText()
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe(currentUser=>this.users=currentUser);
  }

  FilterText(): Observable<UserModel[]>{
    return this.filter.valueChanges.pipe(
      startWith(''),
      map(text => {
        return this.users.filter(user => {
          const term = text.toLowerCase();
          return (user.name.toLowerCase().includes(term))
        });
      })
    );
  }

  ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }
}
