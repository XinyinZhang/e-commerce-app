using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class CustomerBasketDto
    {
        [Required]
        public string Id { get; set; }
        public List<BasketItemDto> Items { get; set; } = new List<BasketItemDto>();
        public int? DeliveryMethodId { get; set; } // optional property
        // at the time user add things to their baskets, they do not have the option
        // to select their deliveryMethod until they get to the checkout process
        public string ClientSecret { get; set; }
        public string PaymentIntentId { get; set; } // will be used by API to update
        // the payment intent
        public decimal ShippingPrice { get; set; }
    }
}