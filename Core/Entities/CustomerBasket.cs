using System.Collections.Generic;

namespace Core.Entities
{
    public class CustomerBasket
    {
        public CustomerBasket()
        {
        }

        public CustomerBasket(string id)
        {
            Id = id;
        }

        // we will let the customer(angular application in this case)
        // generate the unique id
        public string Id { get; set; }
        public List<BasketItem> Items { get; set; } = new List<BasketItem>();
        public int? DeliveryMethodId { get; set; } // optional property
        // at the time user add things to their baskets, they do not have the option
        // to select their deliveryMethod until they get to the checkout process
        public string ClientSecret { get; set; }
        public string PaymentIntentId { get; set; } // will be used by API to update
        // the payment intent
        public decimal ShippingPrice { get; set; }
    }
}