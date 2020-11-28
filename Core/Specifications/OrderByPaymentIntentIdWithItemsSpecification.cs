using Core.Entities.OrderAggregate;

namespace Core.Specifications
{
    // will be used before creating a order
    // since we may have the case where order was successfully created but payment failed at Stripe
    // in this case we want to submit the payment again, but we don't want to recreate the order,
    // what we could do is to check if this order has been created inside database(if we have an order inside database
    // with the corresponding paymentIntentId)
    // this specification aims to do this check(will be used in OrderService)
    public class OrderByPaymentIntentIdSpecification : BaseSpecification<Order>
    {
        public OrderByPaymentIntentIdSpecification(string paymentIntentId)
            : base(o => o.PaymentIntentId == paymentIntentId)
        {
        }
    }
}