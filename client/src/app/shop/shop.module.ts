import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ShopComponent } from './shop.component';
import { ProductItemComponent } from './product-item/product-item.component';
import { SharedModule } from '../shared/shared.module';
import { ProductDetailsComponent } from './product-details/product-details.component';
import { ShopRoutingModule } from './shop-routing.module';



@NgModule({
  declarations: [ShopComponent, ProductItemComponent, ProductDetailsComponent],
  imports: [
    CommonModule,
    SharedModule,
    // RouterModule
    ShopRoutingModule

  ],
  // exports: [ShopComponent]
  // with lazy loading, no longer need to export the ShopComponent, because our app module
  // is no longer responsible for loading this particular component: it's our shop module
  // that's now gonna be responsible for this
})
export class ShopModule { }
