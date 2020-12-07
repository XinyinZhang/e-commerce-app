import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { IProduct } from '../shared/models/product';
import { ShopService } from './shop.service';
import { IBrand } from '../shared/models/brand';
import { IType } from '../shared/models/productType';
import { ShopParams } from '../shared/models/shopParams';

@Component({
  selector: 'app-shop', // to display shop component -> first add shop module to app module
  templateUrl: './shop.component.html',
  styleUrls: ['./shop.component.scss']
})
export class ShopComponent implements OnInit {

  @ViewChild('search', {static: false}) searchTerm: ElementRef;
  products: IProduct[];
  brands: IBrand[];
  types: IType[];
  shopParams: ShopParams;
  totalCount: number; // count of the number of items after all the
  // filters have been applied（product collections中有多少个符合brand&type条件的备选product
  // name 对应display给user的名字
  // value 对应 httprequest中该sorting option的名字
  sortOptions = [
    {name: 'Alphabetical', value: 'name'},
    {name: 'Price: Low to High', value: 'priceAsc'},
    {name: 'Price: High to Low', value: 'priceDesc'}
  ];
  constructor(private shopService: ShopService) {
    this.shopParams = this.shopService.getShopParams();
  }

  ngOnInit(): void{
    this.getProducts(true);
    this.getBrands();
    this.getTypes();
  }


  getProducts(useCache = false): void
  {
     console.log('inside getProducts() method');
    // using shopService to fetch products data
     this.shopService.getProducts(useCache).subscribe(response => { // we don't need to specify the type of response
           // it already know it will get back an IPagination type from the service
           this.products = response.data;
           this.totalCount = response.count;
          //  console.log(this.products);
           }, error => {
      console.log(error);
    });
  }
  getBrands(): void
  {
    this.shopService.getBrands().subscribe(response => {
      // add another option: All(by adding All to the brands array)
      this.brands = [{id: 0, name: 'All'}, ...response];
    }, error => {
      console.log(error);
    });
  }

  getTypes(): void
  {
    this.shopService.getTypes().subscribe(response => {
      // add another option: All(by adding 'All' to the types array)
      this.types = [{id: 0, name: 'All'}, ...response];
    }, error => {
      console.log(error);
    });
  }

   onBrandSelected(brandId: number): void{ // called when user click a particular brand
        // it will assign the brandId of clicked item to brandIdSelected field and
        // call getProducts to return the products with specific id
    const params = this.shopService.getShopParams();
    params.brandId = brandId;
    params.pageNumber = 1; // solve the bug when at page 2/3/... and select a brand
    // everytime we select a brand, we goes to page 1 anyway
    this.shopService.setShopParams(params);
    this.getProducts();
  }

  onTypeSelected(typeId: number): void{
    const params = this.shopService.getShopParams();
    // user click type B --> call onTypeSelected(B.id)
    // --> call getProducts() to get products with typeId = B.id
    params.typeId = typeId;
    params.pageNumber = 1;
    this.shopService.setShopParams(params);
    this.getProducts();
  }

  // called when user click a sort option: 根据选中的sort option，return products in specific order
  onSortSelected(sort: string): void{
    const params = this.shopService.getShopParams();
    params.sort = sort;
    this.shopService.setShopParams(params);
    this.getProducts(); // cache=false
  }

  // called when user click a page: goes to that page and display all the items on that page
  onPageChanged(event: any): void{
    const params = this.shopService.getShopParams();
    if (params.pageNumber !== event){
      params.pageNumber = event; // the event itself should be a pageNumber
      this.shopService.setShopParams(params);
      this.getProducts(true);
    }

  }

  onSearch(): void{
    const params = this.shopService.getShopParams();
    params.search = this.searchTerm.nativeElement.value;
    params.pageNumber = 1;
    this.shopService.setShopParams(params);
    this.getProducts();
  }
  // reset everthing back to unfiltered state
  onReset(): void{
    this.searchTerm.nativeElement.value = '';
    this.shopParams = new ShopParams();
    this.shopService.setShopParams(this.shopParams);
    this.getProducts();
  }

}
