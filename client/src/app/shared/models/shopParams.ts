// store all the params that goes to shopService
export class ShopParams {
  brandId = 0; // by default, active 'All' option
  typeId = 0;
  // fields corresponding to sorting functionalities
  sort = 'name'; // default sorting option: A-Z
  pageNumber = 1;
  pageSize = 6;
  search: string;
}

