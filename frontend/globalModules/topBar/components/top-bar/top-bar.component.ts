import { Component, OnInit } from '@angular/core';
import {Observable} from "rxjs";
import {UserModel} from "../../../../src/models/user/UserModel";
import {UserService} from "../../../../sevices/user.service";
import {AuthService} from "../../../../sevices/auth/auth.service";


@Component({
  selector: 'app-top-bar',
  templateUrl: './top-bar.component.html',
  styleUrls: ['./top-bar.component.scss']
})
export class TopBarComponent implements OnInit {
  isLoggedIn: boolean;

  constructor(private userService: UserService, private authService: AuthService) { }

  ngOnInit(): void {
    this.isLoggedIn = this.authService.isLogin;
  }

}
