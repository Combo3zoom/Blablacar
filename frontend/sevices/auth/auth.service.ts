import {Injectable} from "@angular/core";
import {RegisterRequestModel} from "../../src/models/auth/registerRequestModel";
import {Observable} from "rxjs";
import {HttpClient} from "@angular/common/http";
import {UserModel} from "../../src/models/user/UserModel";
import {environment} from "../../src/environments/environment";
import {LoginRequestModel} from "../../src/models/auth/loginRequestModel";
import {TokenResponseModel} from "../../src/models/tokenResponse/TokenResponseModel";

@Injectable()
export class AuthService{
  public isLogin: boolean = true;

  constructor(private http: HttpClient) {
  }

  register(data: RegisterRequestModel): Observable<UserModel>{
    const url = `${environment.apiUrl}/api/Auth/register`;
    return this.http.post<UserModel>(url, data)
  };

  login(data: LoginRequestModel): Observable<TokenResponseModel>{
    const url = `${environment.apiUrl}/api/Auth/login`;
    return this.http.post<TokenResponseModel>(url, data);
  }
}
