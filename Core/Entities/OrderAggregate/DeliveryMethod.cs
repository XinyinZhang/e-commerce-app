namespace Core.Entities.OrderAggregate
{
    // a delivery methods for our order, 
    //so that the user can select what sort of delivery they're prepared to pay for
    public class DeliveryMethod : BaseEntity
    {
        public string ShortName { get; set; }
        public string DeliveryTime { get; set; }
        public string Description{ get; set; }
        public decimal Price { get; set; }

    }
}