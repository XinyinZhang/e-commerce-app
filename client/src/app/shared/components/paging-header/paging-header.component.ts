import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-paging-header',
  templateUrl: './paging-header.component.html',
  styleUrls: ['./paging-header.component.scss']
})
// this component will be used in multiple components, so export it in Shared module
export class PagingHeaderComponent implements OnInit {
  @Input() pageNumber: number; // receive data from parent element(shopComponent)
  @Input() pageSize: number;
  @Input() totalCount: number;

  constructor() { }

  ngOnInit(): void {
  }

}
