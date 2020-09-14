namespace Core.Specifications
{

    //purpose: set up the parameters that will be passed into product Controller
    public class ProductSpecParams
    {
        private const int MaxPageSize = 50;
        
        //client can overwrite this value to be any value between 1 ~ 50
        private int _pageSize = 6;
        private string _Search;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }
        //by default return the first page
        public int PageIndex{get; set;} = 1; //pageIndex: pageNumber 
                                            //pageSize: how many items in this page
                                            //PageIndex = 2, pageSize = 5: item 6 -- 10
        //optional parameter
        public int? BrandId { get; set; }
        public int? TypeId { get; set; }
        public string Sort { get; set; }
        public string Search 
        { get => _Search; 
          set => _Search = value.ToLower(); //always convert user input to lowercase
        } //allow users to search for products by their name
          

    }
}