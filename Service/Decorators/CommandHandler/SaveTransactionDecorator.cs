using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Persistence;

namespace Service.Decorators.CommandHandler
{
    public class SaveTransactionDecorator<TCommand> : ICommandHandler<TCommand> where TCommand : ICommand
    {
        private readonly ICqsDbContext _dbContext;
        private readonly ILogger _logger;
        private readonly ICommandHandler<TCommand> _decorated;

        public SaveTransactionDecorator(ICqsDbContext dbContext,
            ILogger logger,
            ICommandHandler<TCommand> decorated)
        {
            _dbContext = dbContext;
            _logger = logger;
            _decorated = decorated;
        } 

        public void Handle(TCommand command)
        {
            _decorated.Handle(command);

            try
            {
                _dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                _logger.LogError($"SaveTransaction failed for command - <{typeof(TCommand)}>, <{e.Message}>.");
                throw;
            }
        }
    }
}
