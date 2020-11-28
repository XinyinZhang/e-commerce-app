using System.Threading.Tasks;
using Core.Entities;
using Core.Entities.OrderAggregate;

namespace Core.Interfaces
{
    public interface IPaymentService
    {
        // recall: client creates the payment intent with API(before payment)
        // which contains their basket, and they can update/modify their payment intent
        // whatever they like before actually creating the order
         Task<CustomerBasket> CreateOrUpdatePaymentIntent(string basketId);
         Task<Order> UpdateOrderPaymentSucceeded(string paymentIntentId);
         Task<Order> UpdateOrderPaymentFailed(string paymentIntentId);
    }
}