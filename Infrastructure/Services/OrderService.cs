using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Core.Specifications;

namespace Infrastructure.Services
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentService _paymentService;

        public OrderService(IBasketRepository basketRepo, IUnitOfWork unitOfWork, IPaymentService paymentService)
        {
            _basketRepo = basketRepo;
            _unitOfWork = unitOfWork;
            _paymentService = paymentService;
        }

        public async Task<Order> CreateOrderAsync(string buyerEmail, int deliveryMethodId, 
        string basketId, Address shippingAddress)
        {
            // 1. get basket from the basketRepo
            var basket = await _basketRepo.GetBasketAsync(basketId);
            //(we do not trust the price at this stage, but we trust the item and
            // the quantity in the basket)
            // 2. get the items themselves from the productRepo(to check the 
            // actual price of these items)
            var items = new List<OrderItem>();
            foreach (var item in basket.Items) {
                var productItem = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                // why need this? why not create the orderItem based on product
                // our product may change(either price/name..), 如果根据product create
                // orderItem，那么product changes时orderItem也被迫变化；we don't want the order
                // to change, client付的钱应该是product当时的价钱，而不是change以后的价钱
                //(don't want relation between product and order)
                // so we need another object that record a snapshot of product at this time
                // and let orderItem has relation with this object
                var itemOrderedSnapShot = new ProductItemOrdered(productItem.Id, 
                productItem.Name, productItem.PictureUrl);
                var orderItem = new OrderItem(itemOrderedSnapShot, productItem.Price, item.Quantity);
                items.Add(orderItem);
            }
            // 3. get the delivery method(currently we only have the id of delivery method)
            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);

            // 4. calculate subtotal
            var subtotal = items.Sum(item => item.Price * item.Quantity);
            // check to see if we already have this order inside database(happens when payment fail
            // and order succeed)
            var spec = new OrderByPaymentIntentIdSpecification(basket.PaymentIntentId);
            var existingOrder = await _unitOfWork.Repository<Order>().GetEntityWithSpec(spec);
            // if we do have an existingOrder, means client is repeating their submit order action
            // (because payment to Stripe failed on their first attempt), we will delete the order completely
            // and create a new order
            if (existingOrder != null) {
                _unitOfWork.Repository<Order>().Delete(existingOrder);
                // update payment intent with the current basket contents
                await _paymentService.CreateOrUpdatePaymentIntent(basket.PaymentIntentId);

            }


            // 5. create order
            var order = new Order(items, buyerEmail, shippingAddress, 
            deliveryMethod, subtotal, basket.PaymentIntentId);
            _unitOfWork.Repository<Order>().Add(order); // nothing save to the database at this point
            // 6. save to db
            var result = await _unitOfWork.Complete(); // actually save order to database
            if (result <= 0) return null; // if fail, send null and let orderController generate
            // the error message

            // 7. if order is saved successfully, we can delete the basket now
            // await _basketRepo.DeleteBasketAsync(basketId);
            
            // 8. return order
            return order;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
            return await _unitOfWork.Repository<DeliveryMethod>().ListAllAsync();
        }

        public async Task<Order> GetOrderByIdAsync(int id, string buyerEmail)
        {
            var spec = new OrdersWithItemsAndOrderingSpecification(id, buyerEmail);

            return await _unitOfWork.Repository<Order>().GetEntityWithSpec(spec);
        }

        public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            var spec = new OrdersWithItemsAndOrderingSpecification(buyerEmail);

            return await _unitOfWork.Repository<Order>().ListAsync(spec);
        }
    }
}