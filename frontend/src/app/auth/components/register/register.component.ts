import {Component, OnDestroy, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {AuthService} from "../../../../../sevices/auth/auth.service";
import {UserModel} from "../../../../models/user/UserModel";
import {Router} from "@angular/router";
import {Subject, takeUntil} from "rxjs";


@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit, OnDestroy {
  form: FormGroup;
  user : UserModel;
  private readonly unsubscribe$ = new Subject<void>();

  constructor(private fb: FormBuilder, private authService: AuthService, private router: Router) { }


  ngOnInit(): void {
    this.form = this.fb.group({
      name: ['', Validators.compose([Validators.required,
        Validators.pattern("^[A-z]{1}[a-z]{3,16}$")])],
      password: ['', Validators.compose([Validators.required,
        Validators.pattern('^(?=.*\\d)(?=.*[a-zA-Z])(?=.*[A-Z])(?=.*[-\\#\\$\\.\\%\\&\\*\\_])(?=.*[a-zA-Z]).{8,16}$')])]
    })
  }

  onSubmit(): void{
    this.authService.register(this.form.value).pipe(takeUntil(this.unsubscribe$)).subscribe((currentUser: UserModel)=>{
      console.log('currentUser', currentUser)
      this.router.navigate(['login']);
    })
  }

  ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }
}
