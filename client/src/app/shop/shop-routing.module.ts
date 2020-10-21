import { NgModule } from '@angular/core';
import { ShopComponent } from './shop.component';
import { ProductDetailsComponent } from './product-details/product-details.component';
import { Routes, RouterModule } from '@angular/router';

const routes: Routes = [
  // The path here is set to an empty string, because the path in shop module is
  // already set tp shop
   {path: '', component: ShopComponent}, // /shop
  // route to a specific product with productId: id
  {path: ':id', component: ProductDetailsComponent,
  data: {breadcrumb: {alias: 'productDetails'}}}, // shop/id

];

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forChild(routes) // make these routers only available for child, not available
              // in our app module; forChild() can be used in multiple modules
  ],
  exports: [RouterModule] // we're going to use ShopRoutingModule inside our shop module
})
export class ShopRoutingModule { }
