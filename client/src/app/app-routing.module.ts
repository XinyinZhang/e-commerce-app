import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { ShopComponent } from './shop/shop.component';
import { ProductDetailsComponent } from './shop/product-details/product-details.component';
import { TestErrorComponent } from './core/test-error/test-error.component';
import { ServerErrorComponent } from './core/server-error/server-error.component';
import { NotFoundComponent } from './core/not-found/not-found.component';

const routes: Routes = [
  {path: '', component: HomeComponent}, // home route: localhost:4200(no extra string)
  {path: 'test-error', component: TestErrorComponent},
  {path: 'server-error', component: ServerErrorComponent},
  {path: 'not-found', component: NotFoundComponent},
  // lazy loading: our shop module will only be activated and loaded when we access the shop path
  {path: 'shop', loadChildren: () => import('./shop/shop.module')
.then(mod => mod.ShopModule)},
  {path: '**', redirectTo: '', pathMatch: 'full'} // when somebody types a bad URL,
  // redirect them to home page

];

@NgModule({
  imports: [RouterModule.forRoot(routes)], // forRoot() let Angular knows that the
  // AppRoutingModule is the root routing module, forRoot() is only used once in application
  exports: [RouterModule]
})
export class AppRoutingModule { }
