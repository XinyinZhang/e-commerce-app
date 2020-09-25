import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NavBarComponent } from './nav-bar/nav-bar.component';

// contains all singleton component, ex: navBar
// singleton: a class that allows only a single instance of itself to be created
// and gives access to that created instance.


@NgModule({
  declarations: [NavBarComponent],
  imports: [
    CommonModule
  ],
  exports: [NavBarComponent]
})
export class CoreModule { }
