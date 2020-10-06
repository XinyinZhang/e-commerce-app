import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NavBarComponent } from './nav-bar/nav-bar.component';
import { RouterModule } from '@angular/router';

// contains all singleton component, ex: navBar
// singleton: a class that allows only a single instance of itself to be created
// and gives access to that created instance.


@NgModule({
  declarations: [NavBarComponent],
  imports: [
    CommonModule,
    RouterModule
  ],
  exports: [NavBarComponent]
})
export class CoreModule { }
