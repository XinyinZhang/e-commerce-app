using System;
using System.Linq.Expressions;
using Core.Entities.OrderAggregate;

namespace Core.Specifications
{
    public class OrdersWithItemsAndOrderingSpecification : BaseSpecification<Order>
    {
        // specification for a list of orderItem
        public OrdersWithItemsAndOrderingSpecification(string email) 
        : base(o => o.BuyerEmail == email)
        {
            // these two related properties needed to be included,
            // otherwise, it won't be displayed together with the order
            AddInclude(o => o.OrderItems);
            AddInclude(o => o.DeliveryMethod);
            AddOrderByDescending(o => o.OrderDate); // display a list of order items
            // from  the least recent data to the most recent data
        }

        // specification for a single order
        public OrdersWithItemsAndOrderingSpecification(int id, string email) 
            : base(o => o.Id == id && o.BuyerEmail == email)
        {
            AddInclude(o => o.OrderItems);
            AddInclude(o => o.DeliveryMethod);
        }
    }
}