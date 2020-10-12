import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NavBarComponent } from './nav-bar/nav-bar.component';
import { RouterModule } from '@angular/router';
import { TestErrorComponent } from './test-error/test-error.component';
import { NotFoundComponent } from './not-found/not-found.component';
import { ServerErrorComponent } from './server-error/server-error.component';
import {ToastrModule} from 'ngx-toastr';

// contains all singleton component, ex: navBar
// singleton: a class that allows only a single instance of itself to be created
// and gives access to that created instance.


@NgModule({
  declarations: [NavBarComponent, TestErrorComponent, NotFoundComponent, ServerErrorComponent],
  imports: [
    CommonModule,
    RouterModule,
    ToastrModule.forRoot({
      // where do we want the toast to appear
      positionClass: 'toast-bottom-right',
      // prevent duplicate toast
      preventDuplicates: true
    })
  ],
  exports: [NavBarComponent]
})
export class CoreModule { }
