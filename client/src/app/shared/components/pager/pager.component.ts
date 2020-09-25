import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-pager',
  templateUrl: './pager.component.html',
  styleUrls: ['./pager.component.scss']
})
export class PagerComponent implements OnInit {
  @Input() totalCount: number;
  @Input() pageSize: number;
  @Output() pageChanged = new EventEmitter<number>(); // 每当user click a pageNumber时，我们想把这个
  // pageNumber 发送给shopComponent(parentComponent), 让它来display这一页上的所有products
  constructor() { }

  ngOnInit(): void {
  }

  onPagerChange(event: any): void{ // whenever user click a page, call this method to
    // fire the pageNumber clicked to parentComponent
    this.pageChanged.emit(event.page);
  }

}
