using Domain;

namespace Events.Orders.OrderCreated
{
    public class OrderCreatedEvent
    {
        public OrderCreatedEvent(Order order)
        {
            Order = order;
        }
        
        public Order Order { get; }   
    }
}
