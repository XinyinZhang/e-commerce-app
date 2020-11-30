import {IProduct} from './product';

export interface IPagnination {
    pageIndex: number;
    pageSize: number;
    count: number;
    data: IProduct[];
  }

export class Pagination implements IPagnination {
  pageIndex: number;
  pageSize: number;
  count: number;
  data: IProduct[] = [];
}
