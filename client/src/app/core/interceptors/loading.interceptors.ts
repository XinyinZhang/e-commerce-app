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
        if (req.method === 'POST' && req.url.includes('orders')) {
            return next.handle(req);
        }
        // we don't want loading interceptor shows up for delete action
        if (req.method === 'DELETE') {
            return next.handle(req);
        }
        if (!req.url.includes('emailexists')) {
            // 如果是checkEmailTaken case的延时，turn off the loading spinner
            return next.handle(req);
        }
        this.busyServices.busy();
        // this.busyServices.busy();
        return next.handle(req).pipe(
            // delay(1000),
            finalize(() => { // turn off spinner once the request has fully completed
                this.busyServices.idle();
            })
        );
    }

}
