import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { ShopComponent } from './shop/shop.component';
import { ProductDetailsComponent } from './shop/product-details/product-details.component';
import { TestErrorComponent } from './core/test-error/test-error.component';
import { ServerErrorComponent } from './core/server-error/server-error.component';
import { NotFoundComponent } from './core/not-found/not-found.component';
import { AuthGuard } from './core/guards/auth.guard';

const routes: Routes = [
  {path: '', component: HomeComponent, data: {breadcrumb: 'Home'}}, // home route: localhost:4200(no extra string)
  {path: 'test-error', component: TestErrorComponent, data: {breadcrumb: 'Test Errors'}},
  {path: 'server-error', component: ServerErrorComponent, data: {breadcrumb: 'Server Error'}},
  {path: 'not-found', component: NotFoundComponent, data: {breadcrumb: 'Not Found'}},
  // lazy loading: our shop module will only be activated and loaded when we access the shop path
  {path: 'shop', loadChildren: () => import('./shop/shop.module')
.then(mod => mod.ShopModule), data: {breadcrumb: 'Shop'}},
  {path: 'basket', loadChildren: () => import('./basket/basket.module')
.then(mod => mod.BasketModule), data: {breadcrumb: 'Basket'}},

{path: 'checkout',
canActivate: [AuthGuard],
loadChildren: () => import('./checkout/checkout.module')
.then(mod => mod.CheckoutModule), data: {breadcrumb: 'Checkout'}},
{path: 'account',
canActivate: [AuthGuard],
loadChildren: () => import('./account/account.module')
.then(mod => mod.AccountModule), data: {breadcrumb: {skip: true}}},
{path: 'orders',
canActivate: [AuthGuard],
loadChildren: () => import('./orders/orders.module')
.then(mod => mod.OrdersModule), data: {breadcrumb: 'Orders'}},
  {path: '**', redirectTo: 'not-found', pathMatch: 'full'} // when somebody types a bad URL,
  // redirect them to home page

];

@NgModule({
  imports: [RouterModule.forRoot(routes)], // forRoot() let Angular knows that the
  // AppRoutingModule is the root routing module, forRoot() is only used once in application
  exports: [RouterModule]
})
export class AppRoutingModule { }
