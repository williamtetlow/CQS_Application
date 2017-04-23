using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Service.Decorators.QueryHandler
{
    public class LogQueryExceptionDecorator<TQuery, TResult> : IQueryHandler<TQuery, TResult>
        where TQuery : IQuery<TResult>
    {
        private readonly ILogger _logger;
        private readonly IQueryHandler<TQuery, TResult> _decorated;

        public LogQueryExceptionDecorator(ILogger logger,
            IQueryHandler<TQuery, TResult> decorated)
        {
            _logger = logger;
            _decorated = decorated;
        }

        public TResult Handle(TQuery query)
        {
            try
            {
                return _decorated.Handle(query);
            }
            catch (Exception e)
            {
                _logger.LogError($"Query <{typeof(TQuery)}> failed with exception - <{e.Message}>.");
                throw;
            }
        }
    }
}