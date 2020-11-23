import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
@Injectable()
 export class JwtInterceptor implements HttpInterceptor {
    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        // we want our interceptor to fetch a token from local storage
        const token = localStorage.getItem('token');
        // if we do have a token, we want the interceptor to set the token inside
        // the header of any request comes to API server
        if (token) {
            req = req.clone({ // take our request --> clone it
        // put the token into the header of the cloned request and send this
        // cloned request to API
                setHeaders: {
                    Authorization: `Bearer ${token}`
                }
            });
        }
        return next.handle(req);

    }

 }
