using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Service.Decorators.CommandHandler
{
    public class LogCommandCalledDecorator<TCommand> : ICommandHandler<TCommand> where TCommand : ICommand
    {
        private readonly ILogger _logger;
        private readonly ICommandHandler<TCommand> _decorated;

        public LogCommandCalledDecorator(ILogger logger,
            ICommandHandler<TCommand> decorated)
        {
            _logger = logger;
            _decorated = decorated;
        }

        public void Handle(TCommand command)
        {
            _logger.LogDebug($"Command <{typeof(TCommand)}> called, <{JsonConvert.SerializeObject(command)}");
            
            _decorated.Handle(command);
        }
    }
}
