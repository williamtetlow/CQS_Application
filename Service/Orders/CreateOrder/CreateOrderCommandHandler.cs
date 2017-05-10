using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Microsoft.Extensions.Logging;
using Persistence;
using Service.OrderLines.FindOrderLinesByIds;
using Service.QueryProcessor;

namespace Service.Orders.CreateOrder
{
    public class CreateOrderCommandHandler : ICommandHandler<CreateOrderCommand>
    {
        private readonly ICqsDbContext _dbContext;
        private readonly IQueryProcessor _queryProcessor;
        private readonly ILogger _logger;

        public CreateOrderCommandHandler(ICqsDbContext dbContext, IQueryProcessor queryProcessor, ILogger logger)
        {
            _dbContext = dbContext;
            _queryProcessor = queryProcessor;
            _logger = logger;
        }

        public void Handle(CreateOrderCommand command)
        {
            var order = new Order()
            {
                User = command.User,
                OrderLines = GetOrderLines(command.OrderLineIds),
            };

            _dbContext.Orders.Add(order);
        }

        private ICollection<OrderLine> GetOrderLines(IEnumerable<Guid> ids)
        {
            var idsList = ids as IList<Guid> ?? ids.ToList();

            if (!idsList.Any())
            {
                _logger.LogDebug("CreateOrderCommand - empty list of OrderLine ids creating Order with empty OrderLine list.");

                return new List<OrderLine>();
            }

            var getOrderLinesByIds = new FindOrderLinesByIdsQuery(idsList);

            return _queryProcessor.Process(getOrderLinesByIds).ToList();
        }
    }
}
