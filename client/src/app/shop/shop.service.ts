import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { IPagnination } from '../shared/models/pagination';
import { IBrand } from '../shared/models/brand';
import { IType } from '../shared/models/productType';
import {map} from 'rxjs/operators';
import { ShopParams } from '../shared/models/shopParams';
import { IProduct } from '../shared/models/product';

@Injectable({
  providedIn: 'root'// By default, Angular的CLI
  // command会自动 register service的provider with the ‘root’ injector,
  // means 这个service is available throughout the app
})
// goal of this service: provide data from database
// 现在，我们直接在每个需要data的component里inject httpClient，来
// fetch the data from database --> now 我们可以直接把httpClient inject
// into the service, 让service来fetch data，其他components则直接call service
// 的method来获得这些data
export class ShopService {

  baseUrl = 'https://localhost:5001/api/';
  constructor(private http: HttpClient) { }

  // tslint:disable-next-line: typedef
  getProducts(shopParams: ShopParams){
    let params = new HttpParams();
    if (shopParams.brandId !== 0){
      params = params.append('brandId', shopParams.brandId.toString()); // brandId=5
    }
    if (shopParams.typeId !== 0) {
      params = params.append('typeId', shopParams.typeId.toString()); // typeId=3
    }
    if(shopParams.search){
      params = params.append('search', shopParams.search);
    }

    params = params.append('sort', shopParams.sort); // sort='priceAsc'
    params = params.append('pageIndex', shopParams.pageNumber.toString()); // page number
    params = params.append('pageSize', shopParams.pageSize.toString()); // how many items in that page

    return this.http.get<IPagnination>(this.baseUrl + 'products', {observe: 'response', params})
    .pipe(
      map(response => {
        return response.body;
      })
    );

  }
  // tslint:disable-next-line: typedef
  getBrands(){
    return this.http.get<IBrand[]>(this.baseUrl + 'products/brands');
  }

  // tslint:disable-next-line: typedef
  getTypes(){
    return this.http.get<IType[]>(this.baseUrl + 'products/types');
  }

  // tslint:disable-next-line: typedef
  getProduct(id: number){
    return this.http.get<IProduct>(this.baseUrl + 'products/' + id);
  }

}
