import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class OrdersService {
 baseUrl = environment.apiUrl;
 constructor(private http: HttpClient) { }

 // tslint:disable-next-line: typedef
 getOrdersForUser() {
  return this.http.get(this.baseUrl + 'order');
 }
 // tslint:disable-next-line: typedef
 getOrderDetailed(id: number) {
  return this.http.get(this.baseUrl + 'order/' + id);
 }
}
