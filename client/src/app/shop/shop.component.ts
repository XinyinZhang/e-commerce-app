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
  // search is a static element that's not relying on any dynamic activity
  @ViewChild('search', {static: true}) searchTerm: ElementRef;
  products: IProduct[];
  brands: IBrand[];
  types: IType[];
  shopParams = new ShopParams();
  totalCount: number; // count of the number of items after all the
  // filters have been applied（product collections中有多少个符合brand&type条件的备选product
  // name 对应display给user的名字
  // value 对应 httprequest中该sorting option的名字
  sortOptions = [
    {name: 'Alphabetical', value: 'name'},
    {name: 'Price: Low to High', value: 'priceAsc'},
    {name: 'Price: High to Low', value: 'priceDesc'}
  ];
  constructor(private shopService: ShopService) { }// inject the shop service

  ngOnInit(): void{
    this.getProducts();
    this.getBrands();
    this.getTypes();
  }

  getProducts(): void
  {
    // using shopService to fetch products data
    this.shopService.getProducts(this.shopParams)
    .subscribe(response => { // we don't need to specify the type of response
           // it already know it will get back an IPagination type from the service
           this.products = response.data;
           this.shopParams.pageNumber = response.pageIndex;
           this.shopParams.pageSize = response.pageSize;
           this.totalCount = response.count;
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
    this.shopParams.brandId = brandId;
    this.shopParams.pageNumber = 1; // solve the bug when at page 2/3/... and select a brand
    // everytime we select a brand, we goes to page 1 anyway
    this.getProducts();
  }

  onTypeSelected(typeId: number): void{
    // user click type B --> call onTypeSelected(B.id)
    // --> call getProducts() to get products with typeId = B.id
    this.shopParams.typeId = typeId;
    this.shopParams.pageNumber = 1;
    this.getProducts();
  }

  // called when user click a sort option: 根据选中的sort option，return products in specific order
  onSortSelected(sort: string): void{
    this.shopParams.sort = sort;
    this.getProducts();
  }

  // called when user click a page: goes to that page and display all the items on that page
  onPageChanged(event: any): void{
    if (this.shopParams.pageNumber !== event){
      this.shopParams.pageNumber = event; // the event itself should be a pageNumber
      this.getProducts();
    }

  }

  onSearch(): void{
    this.shopParams.search = this.searchTerm.nativeElement.value;
    this.shopParams.pageNumber = 1;
    this.getProducts();
  }
  // reset everthing back to unfiltered state
  onReset(): void{
    this.searchTerm.nativeElement.value = '';
    this.shopParams = new ShopParams();
    this.getProducts();
  }

}
