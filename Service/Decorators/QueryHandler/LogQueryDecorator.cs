using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Service.Decorators.QueryHandler
{
    public class LogQueryDecorator<TQuery, TResult> : IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
    {
        private readonly ILogger _logger;

        private readonly IQueryHandler<TQuery, TResult> _decorated;

        public LogQueryDecorator(ILogger logger,
            IQueryHandler<TQuery, TResult> decorated)
        {
            _logger = logger;
            _decorated = decorated;
        }

        public TResult Handle(TQuery query)
        {
            _logger.LogDebug($"Query <{typeof(TQuery)}> called, <{JsonConvert.SerializeObject(query)}>.");

            return _decorated.Handle(query);
        }
    }
}
