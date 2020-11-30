import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { IPagnination, Pagination } from '../shared/models/pagination';
import { IBrand } from '../shared/models/brand';
import { IType } from '../shared/models/productType';
import {map} from 'rxjs/operators';
import { ShopParams } from '../shared/models/shopParams';
import { IProduct } from '../shared/models/product';
import { of } from 'rxjs';

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
  products: IProduct[] = [];
  brands: IBrand[] = [];
  types: IType[] = [];
  pagination = new Pagination();
  shopParams = new ShopParams();

  constructor(private http: HttpClient) { }

  // tslint:disable-next-line: typedef
  getProducts(useCache: boolean){
    // if useCache = false, clear products array before get new items from API
    if (useCache === false) {
      this.products = [];
    }
    if (this.products.length > 0 && useCache === true) {
      const pagesReceived = Math.ceil(this.products.length / this.shopParams.pageSize);

      if (this.shopParams.pageNumber <= pagesReceived) {
        this.pagination.data = // get result from memory
          this.products.slice((this.shopParams.pageNumber - 1) * this.shopParams.pageSize,
            this.shopParams.pageNumber * this.shopParams.pageSize);

        return of(this.pagination);
      }
    }

    let params = new HttpParams();
    if (this.shopParams.brandId !== 0){
      params = params.append('brandId', this.shopParams.brandId.toString()); // brandId=5
    }
    if (this.shopParams.typeId !== 0) {
      params = params.append('typeId', this.shopParams.typeId.toString()); // typeId=3
    }
    if (this.shopParams.search){
      params = params.append('search', this.shopParams.search);
    }

    params = params.append('sort', this.shopParams.sort); // sort='priceAsc'
    params = params.append('pageIndex', this.shopParams.pageNumber.toString()); // page number
    params = params.append('pageSize', this.shopParams.pageSize.toString()); // how many items in that page

    return this.http.get<IPagnination>(this.baseUrl + 'products', {observe: 'response', params})
    .pipe(
      map(response => {
        // append the new set of results from API to the existing
        // set of results and store that in products[]
        this.products = [...this.products, ...response.body.data];
        this.pagination = response.body;
        return this.pagination;
      })
    );

  }
  // tslint:disable-next-line: typedef
  getBrands(){
    if (this.brands.length > 0) {
      return of(this.brands);
    }
    // when we get back a brand from server, save it into brands[]
    return this.http.get<IBrand[]>(this.baseUrl + 'products/brands').pipe(
      map(response => {
        this.brands = response;
        return response;
      })
    );
  }

  // tslint:disable-next-line: typedef
  getTypes(){
    if (this.types.length > 0) {
      return of(this.types);
    }
    return this.http.get<IType[]>(this.baseUrl + 'products/types').pipe(
      map(response => {
        this.types = response;
        return response;
      })
    );
  }
  // tslint:disable-next-line: typedef
  getShopParams() {
    return this.shopParams;
  }

  // tslint:disable-next-line: typedef
  setShopParams(params: ShopParams) {
    this.shopParams = params;
  }

  // tslint:disable-next-line: typedef
  getProduct(id: number){
    // before generate http request to ask for a product from API,
    // first check if this product is already inside products[] array
    const product = this.products.find(p => p.id === id);
    if (product) {
      // return an observable of product
      return of(product);
    }
    return this.http.get<IProduct>(this.baseUrl + 'products/' + id);
  }

}
