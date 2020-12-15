import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { IPagnination, Pagination } from '../shared/models/pagination';
import { IBrand } from '../shared/models/brand';
import { IType } from '../shared/models/productType';
import {map} from 'rxjs/operators';
import { ShopParams } from '../shared/models/shopParams';
import { IProduct } from '../shared/models/product';
import { of } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'// By default, Angular's CLI
  // command will automatically register service's provider with the ‘root’ injector,
  // means this service is available throughout the app
})
// goal of this service: provide data from database
// 现在，我们直接在每个需要data的component里inject httpClient，来
// fetch the data from database --> now 我们可以直接把httpClient inject
// into the service, 让service来fetch data，其他components则直接call service
// 的method来获得这些data
export class ShopService {

  baseUrl = environment.apiUrl;
  products: IProduct[] = [];
  brands: IBrand[] = []; // store all brands
  types: IType[] = []; // store all types
  pagination = new Pagination();
  shopParams = new ShopParams(); // everytime user click a brand/type/page number, update shopParams
  productCache = new Map();

  constructor(private http: HttpClient) { }


  // tslint:disable-next-line: typedef
  getProducts(useCache: boolean) {
    /* When use click a brand/type/sorting/type something and click Search/click reset,
    we set useCache = false, since the current data in memory does not meet the new requirements*/
    if (useCache === false) {
      this.productCache = new Map();
    }
    /* we only set useCache = true when user click a page: we want to first go to productCache(memory)
    to check whether we have requested the items of this page before and store them inside memory.*/
    if (this.productCache.size > 0 && useCache === true) {

      if (this.productCache.has(Object.values(this.shopParams).join('-'))) {
        console.log('load from memory');
        this.pagination.data = this.productCache.get(Object.values(this.shopParams).join('-'));

        return of(this.pagination);
      }
    }

    let params = new HttpParams();

    if (this.shopParams.brandId !== 0) {
      params = params.append('brandId', this.shopParams.brandId.toString());
    }

    if (this.shopParams.typeId !== 0) {
      params = params.append('typeId', this.shopParams.typeId.toString());
    }

    if (this.shopParams.search) {
      params = params.append('search', this.shopParams.search);
    }

    params = params.append('sort', this.shopParams.sort);
    params = params.append('pageIndex', this.shopParams.pageNumber.toString());
    params = params.append('pageSize', this.shopParams.pageSize.toString());

    return this.http.get<IPagnination>(this.baseUrl + 'products', {observe: 'response', params})
      .pipe(
        map(response => {
          this.productCache.set(Object.values(this.shopParams).join('-'), response.body.data);
          this.pagination = response.body;

          return this.pagination;
        })
      );
  }

  // tslint:disable-next-line: typedef
  getBrands(){
    if (this.brands.length > 0) { // if we already have items saved in brands[], no need to make the request again
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
