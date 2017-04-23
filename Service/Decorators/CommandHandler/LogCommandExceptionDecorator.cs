using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Service.Decorators.CommandHandler
{
    public class LogCommandExceptionDecorator<TCommand> : ICommandHandler<TCommand> where TCommand : ICommand
    {
        private readonly ILogger _logger;
        private readonly ICommandHandler<TCommand> _decorated;

        public LogCommandExceptionDecorator(ILogger logger,
            ICommandHandler<TCommand> decorated)
        {
            _logger = logger;
            _decorated = decorated;
        }

        public void Handle(TCommand command)
        {
            try
            {
                _decorated.Handle(command);
            }
            catch (Exception e)
            {
                _logger.LogError($"Command <{typeof(TCommand)}> failed with exception - <{e.Message}>.", e);
                throw;
            }
        }
    }
}
