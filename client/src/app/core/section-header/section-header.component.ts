import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { BreadcrumbService } from 'xng-breadcrumb';

@Component({
  selector: 'app-section-header',
  templateUrl: './section-header.component.html',
  styleUrls: ['./section-header.component.scss']
})
export class SectionHeaderComponent implements OnInit {
  breadcrumb$: Observable<any[]>; // breadcrumb comes in an array
  constructor(private bcService: BreadcrumbService) { }

  ngOnInit(): void {
    // the breadcrumbs observable can be obtained from the bcService
    // we obtain the breadcrumbs observable and assign to our field, so that
    // we could use it inside our template
    this.breadcrumb$ = this.bcService.breadcrumbs$;
  }

}
