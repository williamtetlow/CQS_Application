using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Events;
using Microsoft.Extensions.Logging;

namespace Service.Decorators.CommandHandler
{
    public class CommandEventsDispatcherDecorator<TCommand> : ICommandHandler<TCommand> where TCommand : ICommand
    {
        private readonly IEventDispatcher _eventDispatcher;
        private readonly ILogger _logger;
        private readonly ICommandHandler<TCommand> _decorated;

        public CommandEventsDispatcherDecorator(IEventDispatcher eventDispatcher,
            ILogger logger,
            ICommandHandler<TCommand> decorated)
        {
            _eventDispatcher = eventDispatcher;
            _logger = logger;
            _decorated = decorated;
        }

        public void Handle(TCommand command)
        {
            _decorated.Handle(command);

            if (command.CommandCompletedEvents != null &&
                command.CommandCompletedEvents.Any())
            {
                _logger.LogDebug(
                    $"<{command.CommandCompletedEvents.Count}> Event(s) recorded for command: <{command.GetType().Name}>. Dispatching Event(s) Now.");
                _eventDispatcher.Dispatch<IEvent>(command.CommandCompletedEvents);
            }
            else
            {
                _logger.LogDebug($"No Events recorded for command: <{command.GetType().Name}>. Skipping Event Dispatch Process.");
            }
        }
    }
}
