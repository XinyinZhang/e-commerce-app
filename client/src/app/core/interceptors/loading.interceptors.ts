import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent } from '@angular/common/http';
import { Observable } from 'rxjs';
import { BusyService } from '../services/busy.service';
import { Injectable } from '@angular/core';
import { delay, finalize } from 'rxjs/operators';

@Injectable()
export class LoadingInterceptor implements HttpInterceptor {
    constructor(private busyServices: BusyService) {}
    // Note: in order to make use of this interceptor, we need to add this as
    // a provider inside our app module component
    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        this.busyServices.busy();
        return next.handle(req).pipe(
            delay(1000),
            finalize(() => { // turn off spinner once the request has fully completed
                this.busyServices.idle();
            })
        );
    }

}
