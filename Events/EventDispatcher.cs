using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Events
{
    public class EventDispatcher : IEventDispatcher
    {
        private readonly IServiceProvider _serviceProvider;

        public EventDispatcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void Dispatch<TEvent>(IEnumerable<TEvent> events) where TEvent : IEvent
        {
            foreach (var @event in events)
            {
                var handlerType = typeof(IEventHandler<>).MakeGenericType(@event.GetType());

                IEnumerable<dynamic> handlers = _serviceProvider.GetServices(handlerType).ToList();

                if (!handlers.Any())
                {
                    continue;
                };

                foreach (var handler in handlers)
                {
                    handler.Handle((dynamic)@event);
                }
            }

        }
    }
}
