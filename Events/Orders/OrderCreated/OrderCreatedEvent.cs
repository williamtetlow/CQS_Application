using System;
using Domain;

namespace Events.Orders.OrderCreated
{
    public class OrderCreatedEvent : IEvent
    {
        public OrderCreatedEvent(Order order)
        {
            Timestamp = DateTime.UtcNow;
            Order = order;
        }

        public DateTime Timestamp { get; }

        public Order Order { get; }
        
    }
}
