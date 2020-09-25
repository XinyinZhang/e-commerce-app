import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ShopComponent } from './shop.component';
import { ProductItemComponent } from './product-item/product-item.component';
import { SharedModule } from '../shared/shared.module';



@NgModule({
  declarations: [ShopComponent, ProductItemComponent],
  imports: [
    CommonModule,
    SharedModule

  ],
  exports: [ShopComponent]
  // put Component in the exports property of the @NgModule decorator enable
  // an Angular module to expose this component to other modules in the application
  // Without it, the component defined in shopModule could only be used in shopModule
})
export class ShopModule { }
