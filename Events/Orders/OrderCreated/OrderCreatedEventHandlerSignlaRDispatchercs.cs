using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events.Orders.OrderCreated
{
    public class OrderCreatedEventHandlerSignalRDispatcher : IEventHandler<OrderCreatedEvent>
    {
        public OrderCreatedEventHandlerSignalRDispatcher()
        {

        }
        public void Handle(OrderCreatedEvent @event)
        {
            throw new NotImplementedException();
        }
    }
}
