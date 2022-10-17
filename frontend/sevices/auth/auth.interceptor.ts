import {HttpEvent, HttpHandler, HttpInterceptor, HttpRequest} from "@angular/common/http";
import {Observable} from "rxjs";
import {Injectable} from "@angular/core";

@Injectable()
export class AuthInterceptor implements HttpInterceptor{
  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const accessToken = localStorage.getItem('accessToken');
    console.log("accessToken", accessToken);
    if(accessToken){
      req = req.clone({
        setHeaders: {Authorization: `bearer ${accessToken}`},
        withCredentials: true
      });
    }

    return next.handle(req);
  }

}
