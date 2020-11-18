namespace Core.Entities.OrderAggregate
{
    // a snapshot of our product items at the time it was placed
    // based on the fact that our products may change, but we don't want
    // to change it as well inside our orders
    // we dont want relation between our order and product items --> we'll store
    // the values as it was when it was ordered into our database
    public class ProductItemOrdered
    {
        public ProductItemOrdered()
        {
        }

        public ProductItemOrdered(int productItemId, string productName, string pictureUrl)
        {
            ProductItemId = productItemId;
            ProductName = productName;
            PictureUrl = pictureUrl;
        }

        public int ProductItemId { get; set; }
        public string ProductName { get; set; }
        public string PictureUrl { get; set; }
    }
}