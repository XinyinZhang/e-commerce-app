using System.Collections.Generic;

namespace API.Helpers
{
    public class Pagination<T> where T : class 
    {
        public Pagination(int pageIndex, int pageSize, int count, IReadOnlyList<T> data)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            Count = count;
            Data = data;
        }

        // purpose: we want to return the pagination information that the client can make use of: 
        //total counts of item available, which page we are in and what the page size is, the data returned 
        public int PageIndex{get; set;}
        public int PageSize { get; set; }
        public int Count { get; set; } //count of the number of items after all the
                                       //filters have been applied（product collections中有多少个符合brand&type条件的备选product）

     //ex: 符合 brandId = 1， typeId = 3的product备选有多少个
    //if do not apply any filter(do not set brandId & typeId) --> count = 18 --> since there are 18 products in total                            
        public IReadOnlyList<T> Data { get; set; } //store the number of items after all the
                                       //filters have been applied

    }
}