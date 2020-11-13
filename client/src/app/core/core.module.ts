import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NavBarComponent } from './nav-bar/nav-bar.component';
import { RouterModule } from '@angular/router';
import { TestErrorComponent } from './test-error/test-error.component';
import { NotFoundComponent } from './not-found/not-found.component';
import { ServerErrorComponent } from './server-error/server-error.component';
import {ToastrModule} from 'ngx-toastr';
import {BreadcrumbModule} from 'xng-breadcrumb';
import { SectionHeaderComponent } from './section-header/section-header.component';
import { SharedModule } from '../shared/shared.module';

// contains all singleton component, ex: navBar
// singleton: a class that allows only a single instance of itself to be created
// and gives access to that created instance.


@NgModule({
  declarations: [NavBarComponent, TestErrorComponent, NotFoundComponent, ServerErrorComponent, SectionHeaderComponent],
  imports: [
    CommonModule,
    RouterModule,
    BreadcrumbModule,
    SharedModule,
    ToastrModule.forRoot({
      // where do we want the toast to appear
      positionClass: 'toast-bottom-right',
      // prevent duplicate toast
      preventDuplicates: true
    })
  ],
  exports: [
    NavBarComponent,
    SectionHeaderComponent
  ]
})
export class CoreModule { }
