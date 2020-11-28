using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.Extensions.Configuration;
using Stripe;
using Order = Core.Entities.OrderAggregate.Order;
using Product = Core.Entities.Product;

namespace Infrastructure.Services
{
    public class PaymentService : IPaymentService // remember to add this service to servicesExtension
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;
        public PaymentService(IBasketRepository basketRepository, IUnitOfWork unitOfWork,
        
        IConfiguration config)
        {
            _config = config;
            _unitOfWork = unitOfWork;
            _basketRepository = basketRepository;
        }

    public async Task<CustomerBasket> CreateOrUpdatePaymentIntent(string basketId)
    {
        // set Api secret key
        StripeConfiguration.ApiKey = _config["StripeSettings:SecretKey"];
        var basket = await _basketRepository.GetBasketAsync(basketId);
        if (basket == null) return null;
        
        var shippingPrice = 0m;
        if (basket.DeliveryMethodId.HasValue)
        {
            // at this point we don't trust client, so we want to take the deliveryMethod with
            // corresponding deliveryMethodId and fetch the deliveryPrice from our database
            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>()
                    .GetByIdAsync((int)basket.DeliveryMethodId);
            shippingPrice = deliveryMethod.Price;
            
        }
        // check the basket items and confirm the prices are accurate
        foreach (var item in basket.Items)
        {
            // get each product with the corresponding id from database
            var productItem = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
            if(item.Price != productItem.Price)
            { // if item.Price is different from the accurate price, change it back
                item.Price = productItem.Price;
            }
        }
        var service = new PaymentIntentService();
        PaymentIntent intent;
        // check if we're updating a payment intent or creating a new one
        if(string.IsNullOrEmpty(basket.PaymentIntentId))
        {
            // if we currently do not have a payment intent, create one
            var options = new PaymentIntentCreateOptions
                {   // how much the user is intented to pay
                    Amount = (long) basket.Items.Sum(i => i.Quantity * (i.Price * 100)) + (long) shippingPrice * 100,
                    Currency = "usd",
                    PaymentMethodTypes = new List<string> {"card"}
                };
            // create the payment intent with the above option
            // Stripe Service will return back a client secret --> save in intent variable
            intent = await service.CreateAsync(options);
            // update basket's clientSecret variable & paymentIndentId variable
            basket.ClientSecret = intent.ClientSecret;
            basket.PaymentIntentId = intent.Id;
        }
        else {
            // if paymentIndentId is not empty, we want to update this payment Intent
            var options = new PaymentIntentUpdateOptions
                {
                    Amount = (long) basket.Items
                    .Sum(i => i.Quantity * (i.Price * 100)) + (long) shippingPrice * 100 // update the amount to new amount
                };
                await service.UpdateAsync(basket.PaymentIntentId, options);
        }
        // also update the basket inside basket database, which will update the basket
        // to its correct prices, and if it's a new payment intent we've create,
        // the paymentIndentId and client secret field of the basket will also be updated
        await _basketRepository.UpdateBasketAsync(basket);
        return basket;
    }

        public async Task<Order> UpdateOrderPaymentFailed(string paymentIntentId)
        {
            var spec = new OrderByPaymentIntentIdSpecification(paymentIntentId);
            // get the order that failed to be processed by Stripe
            var order = await _unitOfWork.Repository<Order>().GetEntityWithSpec(spec);

            if (order == null) return null;

            // update order to failed status 
            order.Status = OrderStatus.PaymentFailed;
            await _unitOfWork.Complete();

            return order;
        }

        public async Task<Order> UpdateOrderPaymentSucceeded(string paymentIntentId)
        {
            var spec = new OrderByPaymentIntentIdSpecification(paymentIntentId);
            // get the order that has been processed successfully by Stripe
            var order = await _unitOfWork.Repository<Order>().GetEntityWithSpec(spec);
            if (order == null) return null;
            // update order
            order.Status = OrderStatus.PaymentRecevied;
            _unitOfWork.Repository<Order>().Update(order);

            await _unitOfWork.Complete();

            return order;
        }
    }
}