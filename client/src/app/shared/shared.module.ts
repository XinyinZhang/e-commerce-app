import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PaginationModule} from 'ngx-bootstrap/pagination';
import { CarouselModule} from 'ngx-bootstrap/carousel';
import { PagingHeaderComponent } from './components/paging-header/paging-header.component';
import { PagerComponent } from './components/pager/pager.component';

// Note: for anything we add to sharedModule, we also need to export
// because we're gonna to import the sharedModule into any feature modules
// that need the functionalities provided inside this sharedModule
@NgModule({
  declarations: [PagingHeaderComponent, PagerComponent],
  imports: [
    CommonModule,
    PaginationModule.forRoot(),
    CarouselModule.forRoot()
  ],
  exports: [
    PaginationModule,
    PagingHeaderComponent,
    PagerComponent,
  CarouselModule
  ] // make all components/functionalities in shared Module available outside so that
  // any featureModule that need the shared functionality (ex: shopModule) can use them
  // note that: any featureModule should imports the sharedModule first if they want to
  // use functionalities inside sharedModule
})
export class SharedModule { }
