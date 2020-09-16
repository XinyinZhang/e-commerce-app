import {IProduct} from './product';

export interface IPagnination {
    pageIndex: number;
    pageSize: number;
    count: number;
    data: IProduct[];
  }