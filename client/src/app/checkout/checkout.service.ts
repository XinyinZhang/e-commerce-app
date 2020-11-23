import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import {IDeliveryMethod} from '../shared/models/deliveryMethod';
import { IOrderToCreate } from '../shared/models/order';

@Injectable({
  providedIn: 'root'
})
export class CheckoutService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }
  // tslint:disable-next-line: typedef
  creatOrder(order: IOrderToCreate) {
    return this.http.post(this.baseUrl + 'order', order);
  }

  // return a list of delivery methods(sort them based on price)
  // tslint:disable-next-line: typedef
  getDeliveryMethods() {
    return this.http.get(this.baseUrl + 'order/deliveryMethods').pipe(
      map((dm: IDeliveryMethod[]) => {
        return dm.sort((a, b) => b.price - a.price); // sort in higher price first
      })
    );
  }
}
