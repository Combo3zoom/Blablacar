import { Component, OnInit } from '@angular/core';
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {AuthService} from "../../../../../sevices/auth/auth.service";
import {TokenResponseModel} from "../../../../models/tokenResponse/TokenResponseModel";
import {Router} from "@angular/router";
import {Subject, takeUntil} from "rxjs";


@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  form: FormGroup
  private readonly unsubscribe$ = new Subject<void>();

  constructor(private fb: FormBuilder, private authService: AuthService, private router: Router) { }

  ngOnInit(): void {
    this.initializeForm()
  }

  ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }

  initializeForm(): void{
    this.form = this.fb.group({
      name: ['', Validators.compose([Validators.required,
      Validators.pattern("^[A-z]{1}[a-z]{3,16}$")])],
      password: ['', Validators.compose([Validators.required,
        Validators.pattern('^(?=.*\\d)(?=.*[a-zA-Z])(?=.*[A-Z])(?=.*[-\\#\\$\\.\\%\\&\\*\\_])(?=.*[a-zA-Z]).{8,16}$')])]
    })
  }

  onSubmit(): void{
    this.authService.login(this.form.value)
      .pipe(takeUntil(this.unsubscribe$))
      .subscribe((token:TokenResponseModel)=>{
      this.authService.isLogin = true;
      localStorage.setItem('accessToken', token.accessToken);
      this.router.navigate(['all-users']);
    });
  }
}
