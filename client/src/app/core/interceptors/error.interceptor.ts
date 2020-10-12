import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { Injectable } from '@angular/core';
import { Router, NavigationExtras } from '@angular/router';
import { catchError } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';
// goal： catch any error comming back from API, and handle each error

@Injectable() // 代表这个service can be injected; 当angular在 inject 一个service时，
// 它会首先check whether this service is injectable
export class ErrorInterceptor implements HttpInterceptor{
    // inject the router, that will give us access to navigation functionality
    // so that we'll be able to redirect the user to particular ErrorComponent page
    constructor(private router: Router, private toastr: ToastrService) {}
    // next: http response that's comming back
    // goal: we want to catch any error response coming back from API
    // this will give us opportunity to do sth with the particular errors

    // Note: in order to make use of this HTTP interceptor, we need to add this as
    // a provider inside our app module component
    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        return next.handle(req).pipe(
            catchError(error => {
                if (error){
                    if (error.status === 400) {
                        if (error.error.errors) { // if errors[] exist --> 400 validation error
                            throw error.error;
                        }
                        else{ // normal 400 error
                            this.toastr.error(error.error.message, error.error.statusCode);
                        }
                    }
                    if (error.status === 401) {
                        this.toastr.error(error.error.message, error.error.statusCode);
                    }
                    if (error.status === 404){
                        this.router.navigateByUrl('/not-found');
                    }
                    if (error.status === 500){
                        // pass the exception information(error object) to the
                        // component we redirect to
                        // fetch the error object and pass it to the server-error component
                        // via router
                        const navigationExtras: NavigationExtras = {state: {error: error.error}};
                        this.router.navigateByUrl('/server-error', navigationExtras);
                    }
                }
                return throwError(error);
            })
        );
    }

}
